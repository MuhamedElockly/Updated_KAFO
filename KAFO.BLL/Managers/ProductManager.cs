using Kafo.DAL.Repository;
using KAFO.Domain.Products;
using System.Linq.Expressions;

namespace KAFO.BLL.Managers
{
    public class ProductManager : IManager<Product>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Product> GetAll()
        {
            //all product data from repository
            return _unitOfWork.Products.GetAll().ToList();
        }
        public void Add(Product p)
        {
            p.LastPurchasingPrice = p.AveragePurchasePrice;

            _unitOfWork.Products.Add(p);
            _unitOfWork.Save();
        }
        public void Update(Product p)
        {
            //all product data from repository
            _unitOfWork.Products.Update(p);
            _unitOfWork.Save();
        }
        public void Delete(Product p)
        {
            //all product data from repository
            _unitOfWork.Products.Add(p);
            _unitOfWork.Save();
        }

        public void AddORUpdate(int id, Product entity)
        {
            entity.Id = id;
            entity.ImageUrl = entity.ImageUrl ?? Get(p => p.Id == id)?.ImageUrl;
            _unitOfWork.Products.AddORUpdate(id, entity);
            _unitOfWork.Save();
        }

        public void AddRange(IEnumerable<Product> entitie)
        {
            _unitOfWork.Products.AddRange(entitie);
            _unitOfWork.Save();
        }

        public Product? FindById(object id)
        {
            return _unitOfWork.Products.FindById(id);
        }

        public Product? Get(Expression<Func<Product, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            return _unitOfWork.Products.Get(filter, includeProperties, tracked);
        }

        public IEnumerable<Product> GetAll(string? includeProperties = null, Expression<Func<Product, bool>>? filter = null)
        {
            return _unitOfWork.Products.GetAll(includeProperties, filter);

        }
        public string Delete(int productId)
        {
            Product product = _unitOfWork.Products.Get(p => p.Id == productId);
            if (product != null)
            {
                if (product.StockQuantity > 0)
                {
                    return "لا يمكن حذف المنتج لانه معروض بالفعل";
                }
                else if (product.StockQuantity == 0)
                {
                    _unitOfWork.Products.Remove(product);
                    _unitOfWork.Save();
                }

            }
            else
            {
                return "المنتج غير موجود";
            }
            return null;
        }
        //public void Remove(Product entity)
        //{
        //    _unitOfWork.Product.Remove(entity);
        //    _unitOfWork.Save();
        //}

        public void RemoveRange(IEnumerable<Product> entities)
        {
            _unitOfWork.Products.RemoveRange(entities);
            _unitOfWork.Save();
        }

    }
}
