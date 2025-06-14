
using Kafo.DAL.Data;
using KAFO.Domain.Products;

namespace Kafo.DAL.Repository
{
    public class ProductRepsitory : Repository<Product>, IProductRepository
    {
        private readonly AppDBContext _dbcontext;
        public ProductRepsitory(AppDBContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
