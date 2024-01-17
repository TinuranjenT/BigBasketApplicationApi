using BigBasketApplication.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BigBasketApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository _repository;
        public CustomerController(IRepository repository)
        {
            this._repository = repository;
        }

        [HttpGet("GetAllProductsByCustomer")]
        public async Task<ActionResult<List<Product>>> GetAllProductsByCustomer()
        {
            try
            {
                var products = await _repository.GetAllProductsByCustomer();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("AddToCart")]
        public async Task<ActionResult<Cart>> AddToCart(Cart cart)
        {
            try
            {
                var updatedCart = await _repository.AddToCart(
                cart.CustomerId,
                cart.CartItems.FirstOrDefault().ProductId,
                cart.CartItems.Sum(item => item.Quantity)
                );
                if (updatedCart != null)
                {
                    return Ok(updatedCart);
                }
                else
                {
                    return BadRequest("Product is not available for the entered quantity");
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");

                return BadRequest($"Failed to add product to the cart: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add product to the cart: {ex.Message}");
            }
        }
 

        [HttpGet("GetAllProductsFromCart")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllProductsFromCart()
        {
            try
            {
                var carts = await _repository.GetAllProductsFromCart();

                if (carts != null)
                {
                    return Ok(carts);
                }
                else
                {
                    return NotFound("No carts found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GenerateBill/{customerId}")]
        public async Task<ActionResult<int>> GenerateBill(int customerId)
        {
            try
            {
                var order = await _repository.GenerateBill(customerId);

                if (order != null)
                {
                    return Ok(order);
                }
                else
                {
                    return NotFound($"No items found in the cart for customer with ID {customerId}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while generating the bill: {ex.Message}");
            }
        }

    }



}


