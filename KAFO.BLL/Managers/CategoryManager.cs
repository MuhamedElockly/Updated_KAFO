using Kafo.DAL.Repository;
using KAFO.Domain.Products;

namespace KAFO.BLL.Managers
{
    public class CategoryManager : IManager<Category>
    {
        IUnitOfWork UOW = null;
        public CategoryManager(IUnitOfWork _unitOfWork)
        {
            UOW = _unitOfWork;
        }


        public List<Category> GetAll()
        {
            //all product data from repository
            return UOW.Categories.GetAll().ToList();
        }
        public void Add(Category p)
        {
            //all product data from repository
            UOW.Categories.Add(p);
            UOW.Save();
        }
        public Category Get(int id)
        {
            return UOW.Categories.FindById(id);

        }
        public void Update(Category p)
        {
            //all product data from repository
            UOW.Categories.Update(p);
            UOW.Save();
        }
        public void Delete(Category p)
        {
            //all product data from repository
            UOW.Categories.Remove(p);
            UOW.Save();
        }
    }
}