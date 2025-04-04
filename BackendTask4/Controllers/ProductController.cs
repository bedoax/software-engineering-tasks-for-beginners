using BackendTask4.Model;
using BackendTasks.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BackendTask4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        [Route("")]
        public IActionResult GetAllProduct()
        {
            var products = _dbContext.Products;
            if (products is null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product is null)
            {
                return NotFound();
            }
            //var category = product.ProductCategories.Select(p => p.Category.Name).ToList();
            
            return Ok(product);
        }
        [HttpPost]
        [Route("")]
        public IActionResult PostProduct(ProductDto productv1)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isExist = _dbContext.Products.SingleOrDefault(p => p.Name == productv1.Name);
            if (isExist != null)
            {
                return BadRequest(new { message = "Product already exists" });
            }
            var product = new Product
            {
                Name = productv1.Name,
                Price = productv1.Price,
                Description = productv1.Description,
                Stock = productv1.Stock,
                ProductCategories = new List<ProductCategory>()
            };
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            return Ok(new { product });
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult PutProduct(ProductDto product,int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isExist = _dbContext.Products.Find(id);
            if (isExist is null)
            {
                return NotFound();
            }
            isExist.Name = product.Name;
            isExist.Price = product.Price;
            _dbContext.SaveChanges();
            return Ok(new { isExist });
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteProduct(int id)
        {

            var isExist = _dbContext.Products.Find(id);
            if (isExist is null)
            {
                return NotFound();
            }
            var productCategory = _dbContext.ProductCategories.Where(pc => pc.ProductId == id).ToList();
            _dbContext.ProductCategories.RemoveRange(productCategory);
            var orderItem = _dbContext.OrderItems.Where(oi => oi.ProductId == id).ToList();
            _dbContext.OrderItems.RemoveRange(orderItem);

            _dbContext.Products.Remove(isExist);
            _dbContext.SaveChanges();
            return Ok(new { message = "Product deleted" });
        }
    }
}
