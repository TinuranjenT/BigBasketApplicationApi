using BigBasketApplication.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BigBasketApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IRepository _repository;
        public AdminController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetAllProductsByAdmin")]

        public async Task<ActionResult<List<Product>>> GetAllProductsByAdmin()
        {
            var products = await _repository.GetAllProductsByAdmin();
            return Ok(products);

        }

        [HttpGet("GetProductById/{productId}")]

        public async Task<ActionResult<Product>> GetProductById(int productId)
        {
            var products = await _repository.GetProductById(productId);
            return Ok(products);
        }

        

        [HttpPost("PostNewProduct")]
        public async Task<ActionResult> PostNewProduct(Product product)
        {
            try
            {
                await _repository.PostNewProduct(product);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while processing the request.");
            }
        }

        [HttpPost("RefillTheProduct")]
        public async Task<ActionResult<Product>> RefillStock(int productId, int quantityToBeAdd)
        {
            try
            {
                var result = await _repository.RefillStock(productId, quantityToBeAdd);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound("Unable to refill the stock");
            }

        }

        

        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            try
            {
                await _repository.DeleteProduct(productId);
                return Ok("Product has been successfully deleted");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
             

        }


    }
}
