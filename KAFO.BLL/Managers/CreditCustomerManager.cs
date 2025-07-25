
using KAFO.Domain.Users;
using KAFO.Domain.Invoices;
using Kafo.DAL.Repository;
using System.Collections.Generic;
using System.Linq;
using KAFO.BLL.Managers;
using System;

namespace KAFO.BLL.Managers
{
    public class CreditCustomerManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreditCustomerManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<CustomerAccount> GetAll()
        {
            return _unitOfWork.CustomerAccounts.GetAll().ToList();
        }

        public CustomerAccount Get(int id)
        {
            return _unitOfWork.CustomerAccounts.FindById(id);
        }

        public void Add(CustomerAccount customer)
        {
            _unitOfWork.CustomerAccounts.Add(customer);
            _unitOfWork.Save();
        }

        public void Update(CustomerAccount customer)
        {
            _unitOfWork.CustomerAccounts.Update(customer);
            _unitOfWork.Save();
        }

        public string Delete(int id)
        {
            var customer = Get(id);
            if (customer != null)
            {

                if (customer.TotalOwed != 0)
                {
                    return "لا يمكن حذف العميل لأن لديه رصيد غير مسدد.";
                }
                if (customer.Balance != 0)
                {
                    return "لا يمكن حذف العميل لأن لديه أموال في الحساب.";
                }
                _unitOfWork.CustomerAccounts.Remove(customer);
                _unitOfWork.Save();
                return null;
            }
            return "العميل غير موجود.";
        }

        public bool PhoneExists(string phone, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return _unitOfWork.CustomerAccounts.GetAll()
                    .Any(c => c.PhoneNumber == phone && c.Id != excludeId.Value);
            }
            return _unitOfWork.CustomerAccounts.GetAll()
                .Any(c => c.PhoneNumber == phone);
        }

        //public CreditCustomerAccountVM GetAccountVM(int customerId)
        //{
        //    var customer = _unitOfWork.CustomerAccounts.FindById(customerId);
        //    if (customer == null) return null;

        //    // Get all invoices for this customer, including the seller (User)
        //    var invoices = _unitOfWork.Invoices.GetAll(includeProperties: "User", filter: i => i.CustomerAccountId == customerId);

        //    var transactions = invoices
        //        .OrderByDescending(i => i.CreatedAt)
        //        .Select(i => new CreditCustomerTransactionVM
        //        {
        //            SellerName = i.User?.Name ?? "غير معروف",
        //            Time = i.CreatedAt,
        //            DepositMoney = i.TotalInvoice
        //        }).ToList();

        //    return new CreditCustomerAccountVM
        //    {
        //        Id = customer.Id,
        //        Name = customer.CustomerName,
        //        Phone = "", // Add real phone if available
        //        Email = null, // Add real email if available
        //        Gender = "", // Add real gender if available
        //        Balance = customer.Balance,
        //        Credit = customer.TotalOwed,
        //        Transactions = transactions,
        //        CurrentPage = 1,
        //        TotalPages = 1 // Client-side pagination
        //    };
        //}

        public CreditCustomerAccountData GetWithInvoicesData(int customerId)
        {
            var customer = _unitOfWork.CustomerAccounts.Get(
                c => c.Id == customerId,
                includeProperties: "Deposits,Deposits.User"
            );
            if (customer == null) return null;
            var transactions = customer.Deposits != null
                ? customer.Deposits.OrderByDescending(i => i.CreatedAt)
                    .Select(i => new CreditCustomerTransactionData
                    {
                        SellerName = i.User?.Name ?? "غير معروف",
                        Time = i.CreatedAt,
                        DepositMoney = i.TotalInvoice
                    }).ToList()
                : new List<CreditCustomerTransactionData>();
            return new CreditCustomerAccountData
            {
                Id = customer.Id,
                Name = customer.CustomerName,
                Phone = customer.PhoneNumber ?? "",
                Email = customer.Email,
                Gender = customer.Gender ?? "",
                Balance = customer.Balance,
                Credit = customer.TotalOwed,
                Transactions = transactions
            };
        }

        public SettlementResult SettleAccount(int customerId, decimal amount, int userId, bool? withdraw)
        {
            try
            {
                if (amount <= 0)
                {
                    return new SettlementResult
                    {
                        Success = false,
                        Message = "يجب أن يكون المبلغ أكبر من صفر"
                    };
                }

                var customer = _unitOfWork.CustomerAccounts.FindById(customerId);
                if (customer == null)
                {
                    return new SettlementResult
                    {
                        Success = false,
                        Message = "العميل غير موجود"
                    };
                }

                var currentUser = _unitOfWork.Users.FindById(userId);
                if (currentUser == null)
                {
                    return new SettlementResult
                    {
                        Success = false,
                        Message = "لم يتم العثور على المستخدم الحالي"
                    };
                }
                var oldTotalOwed = customer.TotalOwed;
                var oldTotalPaid = customer.TotalPaid;
                var message = "";

                if (withdraw == null)
                {
                    // Check if customer has debt to settle
                    if (customer.TotalOwed > 0)
                    {
                        var currentDebt = customer.TotalOwed;

                        // If amount is greater than debt, settle debt first, then add remaining as credit
                        if (amount >= currentDebt)
                        {
                            var remainingAmount = amount - currentDebt;

                            // Settle all debt
                            customer.SettleDebt(currentDebt);

                            // Add remaining as credit (TotalPaid)
                            if (remainingAmount > 0)
                            {
                                customer.AddPayment(remainingAmount);
                            }

                            message = $"تم تسوية دين العميل {customer.CustomerName} بالكامل ({currentDebt:C}) وإضافة رصيد إضافي ({remainingAmount:C})";
                        }
                        else
                        {
                            // Settle partial debt
                            customer.SettleDebt(amount);
                            message = $"تم تسوية جزء من دين العميل {customer.CustomerName} بمبلغ {amount:C}";
                        }
                    }
                    else
                    {
                        // No debt, add as credit
                        customer.AddPayment(amount);
                        message = $"تم إضافة رصيد للعميل {customer.CustomerName} بمبلغ {amount:C}";
                    }
                    var creditTerminateInvoice = new CreditTerminateInvoice
                    {
                        CreatedAt = DateTime.Now,
                        User = currentUser,
                        CustomerAccount = customer,
                        TotalInvoice = amount
                    };

                    _unitOfWork.CreditTerminateInvoice.Add(creditTerminateInvoice);

                    // Create credit terminate invoice
                }
                else if (withdraw == true)
                {
                    if (customer.TotalOwed > 0)
                    {
                        return new SettlementResult
                        {
                            Success = false,
                            Message = "لا يمكن سحب الأموال من العميل لأنه لديه دين مستحق"
                        };
                    }
                    else if (customer.Balance < amount)
                    {
                        return new SettlementResult
                        {
                            Success = false,
                            Message = "لا يوجد رصيد كافي للسحب"
                        };
                    }
                    else
                    {

                        customer.WithdrawBalance(amount);
                        message = $"تم سحب مبلغ {amount:C} من رصيد العميل {customer.CustomerName}";
                        var creditWithdrawInvoice = new CreditWithdrawInvoice
                        {
                            CreatedAt = DateTime.Now,
                            User = currentUser,
                            CustomerAccount = customer,
                            TotalInvoice = amount
                        };

                        _unitOfWork.CreditWithdrawInvoices.Add(creditWithdrawInvoice);
                    }
                }

                _unitOfWork.Save();

                return new SettlementResult
                {
                    Success = true,
                    Message = message,
                    CustomerId = customer.Id,
                    NewTotalOwed = customer.TotalOwed,
                    NewTotalPaid = customer.TotalPaid,
                    OldTotalOwed = oldTotalOwed,
                    OldTotalPaid = oldTotalPaid,
                    CustomerName = customer.CustomerName,
                    AmountPaid = amount
                };
            }
            catch (ArgumentException ex)
            {
                return new SettlementResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new SettlementResult
                {
                    Success = false,
                    Message = "حدث خطأ أثناء تصفية الحساب: " + ex.Message
                };
            }
        }
    }
}