using BigBasketApplication.Model;
using Microsoft.AspNetCore.Mvc;

namespace BigBasketApplication
{
    public interface IRepository
    {
        Task<List<Product>> GetAllProductsByAdmin();
        Task<Product> GetProductById(int productId);
        Task<Product> RefillStock(int productId, int quantityToBeAdd);
        Task PostNewProduct(Product product);
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
