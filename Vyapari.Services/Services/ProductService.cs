using Vyapari.Data;
using Vyapari.Data.Entities;

namespace Vyapari.Service;
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateProduct(Product product)
    {
        await _repository.AddProductAsync(product);
    }

    public async Task DeleteProduct(int id)
    {
        await _repository.DeleteProductAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _repository.GetAllProductsAsync();
    }

    public async Task<Product> GetProductById(int id)
    {
        return await _repository.GetProductByIdAsync(id);
    }

    public async Task UpdateProduct(int id, Product product)
    {
        await _repository.UpdateProductAsync(product);
    }
}
