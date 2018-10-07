using System.Collections.Generic;
using System.Threading.Tasks;

namespace RyuWebApiRest.Services.Production
{
    public interface IProductService
    {
        ICollection<Product> GetProducts();
        Task<Product> GetProduct(int prodId);
        void DeleteProduct(int productId);
        int AddProduct(Product product);
        void UpdateProduct(Product product);
    }
}
