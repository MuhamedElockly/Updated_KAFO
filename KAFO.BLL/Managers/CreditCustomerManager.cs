
using KAFO.Domain.Users;
using Kafo.DAL.Repository;
using System.Collections.Generic;
using System.Linq;
using KAFO.BLL.Managers;

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
                if (customer.Balance!=0)
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
    }
} 