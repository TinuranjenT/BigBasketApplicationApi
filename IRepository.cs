using BigBasketApplication.Model;
using Microsoft.AspNetCore.Mvc;

namespace BigBasketApplication
{
    public interface IRepository
    {
        Task<List<Product>> GetAllProductsByAdmin();
        Task<Product> GetProductById(int productId);
        Task<Product> RefillStock(int productId, int quantityToBeAdd);
        Task<Product>PostNewProduct(Product product);
        Task<Product> UpdateTheProduct(int productId, Product updatedProduct);
        Task DeleteProduct(int productId);
        /* Cart AddToCart(int customerId, int productId, int quantity);*/

        // Task<List<Product>> GetAllProductsByCustomer();
        Task<List<ProductForCustomer>> GetAllProductsByCustomer();
        Task<Cart> AddToCart(int customerId, int productId, int quantity);
        Task<List<Cart>> GetAllProductsFromCart();
        Task<Order> GenerateBill(int customerId);

      

        //Task<Product> AddProductAdmin(string name, string price, int stockQuantity);
        //Task<Product> AddProductUser(string name, int stockQuantity);
    }
}
