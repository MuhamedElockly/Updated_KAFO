using Kafo.DAL.Data;
using Kafo.DAL.Repository;
using KAFO.Domain.Products;

namespace KAFO.BLL.Managers
{
	public class CategoryManager
	{
		IUnitOfWork UOW = null;
		public CategoryManager(IUnitOfWork _unitOfWork)
		{
			UOW = _unitOfWork;
		}


		public List<Category> GetAll()
		{
			//all product data from repository
			return UOW.Category.GetAll().ToList();
		}
		public void Add(Category p)
		{
			//all product data from repository
			UOW.Category.Add(p);
			UOW.Save();
		}
		public Category Get(int id)
		{
			return UOW.Category.FindById(id);

		}
		public void Update(Category p)
		{
			//all product data from repository
			UOW.Category.Update(p);
			UOW.Save();
		}
		public void Delete(Category p)
		{
			//all product data from repository
			UOW.Category.Add(p);
			UOW.Save();
		}
	}
}
