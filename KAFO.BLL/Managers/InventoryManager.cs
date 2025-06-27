using Kafo.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAFO.BLL.Managers
{
	public class InventoryManager
	{
		IUnitOfWork UOW = null;
		public InventoryManager(IUnitOfWork _unitOfWork)
		{
			UOW = _unitOfWork;
		}
	}
}
