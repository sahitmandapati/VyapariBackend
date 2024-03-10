using Vyapari.Data.Entities;

namespace Vyapari.Service
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task DeleteProduct(int id);

        Task CreateProduct(Product product);
        Task UpdateProduct(int id, Product product);
    }
}