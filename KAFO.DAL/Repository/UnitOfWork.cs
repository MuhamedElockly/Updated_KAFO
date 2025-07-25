﻿using Kafo.DAL.Data;
using KAFO.DAL.Repository;
using KAFO.DAL.Repository.IRepository;

namespace Kafo.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        AppDBContext _dbcontext;
        public ICategoryRepository Categories { get; private set; }
        public IProductRepository Products { get; private set; }

        public ICustomerAccountRepository CustomerAccounts { get; private set; }

        public IInvoiceItemRepository InvoiceItems { get; private set; }

        public IInvoiceRepository Invoices { get; private set; }

        public IUserRepository Users { get; private set; }

        public ICreditTerminateInvoice CreditTerminateInvoice { get; private set; }
        public ICreditWithdrawInvoice CreditWithdrawInvoices { get; private set; }

        public UnitOfWork(AppDBContext dbContext)
        {
            _dbcontext = dbContext;
            Categories = new CategoryRepository(_dbcontext);
            Products = new ProductRepsitory(_dbcontext);

            CustomerAccounts = new CustomerAccountRepsitory(_dbcontext);
            InvoiceItems = new InvoiceItemRepsitory(_dbcontext);
            Invoices = new InvoiceRepository(_dbcontext);
            Users = new UserRepository(_dbcontext);
            CreditTerminateInvoice = new CreditTerminateInvoiceRepository(_dbcontext);
            CreditWithdrawInvoices = new CreditWithdrawInvoiceRepository(_dbcontext);
        }

        public void Save()
        {
            _dbcontext.SaveChanges();
        }
    }
}
