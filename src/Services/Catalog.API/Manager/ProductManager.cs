using Catalog.API.Models;
using MongoRepo.Manager;
using Catalog.API.Interfaces.Manager;
using Catalog.API.Interfaces.Repository;
using Catalog.API.Repository;

namespace Catalog.API.Manager
{
    public class ProductManager : CommonManager<Product>, IProductManager
    {
        public ProductManager() : base(new ProductRepository())
        {
        }

        public List<Product> GetByCategory(string category)
        {
            return GetAll(c => c.Category == category).ToList();
        }
    } 
}


