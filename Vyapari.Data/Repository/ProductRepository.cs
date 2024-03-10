using Microsoft.EntityFrameworkCore; 
using Vyapari.Data.Entities;

namespace Vyapari.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly VyapariDBContext _context;

        public ProductRepository(VyapariDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            var entry = await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var entry = _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}