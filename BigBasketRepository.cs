using BigBasketApplication.Data;
using BigBasketApplication.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BigBasketApplication
{
    public class BigBasketRepository: IRepository
    {
        private readonly DatabaseContext databaseContext;

        public BigBasketRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task<List<Product>> GetAllProductsByAdmin()
        {
             return await databaseContext.Products.ToListAsync();
        }
        public async Task <List<ProductForCustomer>> GetAllProductsByCustomer()
        {
            var customerProductsList = new List<ProductForCustomer>();
            foreach (var product in databaseContext.Products)
            {
                var customerProduct = new ProductForCustomer
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    GstPercentage = product.GstPercentage,
                    DiscountPercentage = product.DiscountPercentage,
                };
                customerProductsList.Add(customerProduct);  
            }
            return customerProductsList;
           
        }
        public async Task<Product> GetProductById(int productId)
        {
            return await databaseContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
        }

        public async Task PostNewProduct(Product product)
        {
            try
            {
                databaseContext.Products.Add(product);
                await databaseContext.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public async Task<Product> RefillStock(int productId, int quantityToBeAdd)
        {
            var product = await databaseContext.Products.FindAsync(productId);
            if(product != null)
            {
                product.StockQuantity += quantityToBeAdd;
                await databaseContext.SaveChangesAsync();
                
            }
            return product;
        }

        public async Task DeleteProduct(int productId)
        {

            Product product = await databaseContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if(product != null)
            {
                databaseContext.Products.Remove(product);
                await databaseContext.SaveChangesAsync();
            }
        }
        public async Task<Cart> AddToCart(int customerId, int productId,int quantity)
        {
            int available = GetProductAvailability(productId);

            if (available >= quantity)
            {
                var cart = await databaseContext.Carts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId);

                if (cart == null)
                {
                    cart = new Cart { CustomerId = customerId, Items = new List<CartItem>() };
                    databaseContext.Carts.Add(cart);
                }

                var existingItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);

                if (existingItem == null)
                {
                    cart.Items.Add(new CartItem
                    {
                        ProductId = productId,
                        Quantity = quantity,
                       /* ProductName = productName, */
                    });
                }
                else
                {
                    existingItem.Quantity += quantity;
                }

                var product = await databaseContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
                if(product != null)
                {
                    product.StockQuantity -= quantity;
                }

                await databaseContext.SaveChangesAsync();

                return cart;
            }
            else
            {
                Console.WriteLine($"Product {productId} is not available for the entered quantity");
                return null;
            }
        }

        public int GetProductAvailability(int productId)
        {
            var product = databaseContext.Products.FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                return product.StockQuantity;
            }
            else
            {
                return 0;
            }
        }


        public async Task<List<Cart>>GetAllProductsFromCart()
        {
            return await databaseContext.Carts.Include(c => c.Items).ToListAsync();
        }

        public async Task<Order> GenerateBill(int customerId)
        {
           var cart = await databaseContext.Carts
           .Include(c => c.Items)
           .FirstOrDefaultAsync(c => c.CustomerId == customerId);

           if (cart == null || cart.Items.Count == 0)
           {
               return null;
           }

            double totalAmount = 0;
            //double individualAmount = 0;

            var finalOrder = new Order
            {
                CustomerId = customerId,
                DelieverySlot = DateTime.Now.AddDays(2),
                Items = new List<OrderItem>(),
            };
            foreach (var cartItem in cart.Items)
            {
                double productPrice = await GetProductPrice(cartItem.ProductId);
                int quantity = await GetProductQuantity(cartItem.ProductId);
                double baseAmount = quantity * productPrice;
                double gstpercentage = await GetGstPrecentage(cartItem.ProductId);
                // individualAmount = quantity * productPrice;
                double gstAmount = baseAmount * (gstpercentage / 100);

                double discountPercentage = await GetDiscountPrecentage(cartItem.ProductId);

                double discountAmount = baseAmount * (discountPercentage / 100);
                double discountedAmount = baseAmount - discountAmount;
                double individualAmount = discountedAmount + gstAmount;
                totalAmount += individualAmount;

                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Price = individualAmount,
                    Quantity = quantity,
                };
                finalOrder.Items.Add(orderItem);
                databaseContext.CartItems.Remove(cartItem);
            }
            finalOrder.TotalAmount = totalAmount;
            databaseContext.Orders.Add(finalOrder);

            databaseContext.Carts.Remove(cart);
          
            await databaseContext.SaveChangesAsync();
            return finalOrder;
        }

        public async Task<double> GetProductPrice(int productId)
        {
            Product product = await databaseContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (product != null)
            {
                return product.Price;
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> GetProductQuantity(int productId)
        {
            CartItem cartItem = await databaseContext.CartItems.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (cartItem != null)
            {
                return cartItem.Quantity;
            }
            else
            {
                return 0;
            }
        }

        public async Task<double> GetGstPrecentage(int productId)
        {
            Product product = await databaseContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (product != null)
            {
                return product.GstPercentage;
            }
            else
            { 
                return 0; 
            }
        }

        public async Task<double> GetDiscountPrecentage(int productId)
        {
            Product product = await databaseContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (product != null)
            {
                return product.DiscountPercentage;
            }
            else
            {
                return 0;
            }
        }


    }
}
