using Kafo.DAL.Data;
using Kafo.DAL.Repository;
using KAFO.Domain.Products;

namespace KAFO.BLL.Managers
{
    public static class ProductManager
    {
        static AppDBContext appDB = new AppDBContext();
        static UnitOfWork UOW = new UnitOfWork(appDB);
        public static List<Product> GetAll()
        {
            //all product data from repository
            return UOW.Product.GetAll().ToList();
        }
        public static void Add(Product p)
        {
            //all product data from repository
            UOW.Product.Add(p);
            UOW.Save();
        }
        public static void Update(Product p)
        {
            //all product data from repository
            UOW.Product.Update(p);
            UOW.Save();
        }
        public static void Delete(Product p)
        {
            //all product data from repository
            UOW.Product.Add(p);
            UOW.Save();
        }
    }
}
