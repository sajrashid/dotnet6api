using MyAPI.Models;

namespace MyAPI.Repository
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetAllProducts();
        public Task<Product> GetProduct(int id);
        public Task<Product> InsertProduct(Product productToInsert);
        public Task<int> UpdateProduct(int id, Product productToUpDate);
        public Task<int> DeleteProduct(int id);
    }
}
