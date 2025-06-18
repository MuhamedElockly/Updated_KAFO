using Kafo.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAFO.BLL.Managers
{
	public class InvoiceManager
	{
		IUnitOfWork UOW = null;
		public InvoiceManager(IUnitOfWork _unitOfWork)
		{
			UOW = _unitOfWork;
		}
		public double GetTotalProfit(DateTime startDate, DateTime endDate)
		{
			//var invoices = UOW.Invoice.GetAll()
	  // .Where(i => i.CreatedAt >= startDate && i.CreatedAt <= endDate)
	  // .ToList();
			//decimal totalProfit = invoices.Sum(i => i.ToTalInvoiceProfit);

			return (double)45.8;
		}

	}
}
