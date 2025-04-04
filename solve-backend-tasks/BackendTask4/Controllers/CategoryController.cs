using BackendTask4.Model;
using BackendTasks.Data;
using Microsoft.AspNetCore.Mvc;

namespace BackendTask4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("")]
        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isExist = _dbContext.Categories.SingleOrDefault(c => c.Name == category.Name);
            if (isExist != null)
            {
                return BadRequest(new { message = "Category already exists" });
            }
            var test = new Category
            {
                Name = category.Name,
                ProductCategories = new List<ProductCategory>()
            };
          
            _dbContext.Categories.Add(test);
            _dbContext.SaveChanges();
            return Ok(new { category });
        }
        [HttpGet]
        [Route("")]
        public IActionResult GetAllCategory()
        {
            var categories = _dbContext.Categories;
            if (categories is null)
            {
                return NotFound();
            }
            return Ok(categories);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingCategory = _dbContext.Categories.Find(id);
            if (existingCategory is null)
            {
                return NotFound();
            }
            if (category.Name == existingCategory.Name)
            {
                return BadRequest(new { message = "Category already exists" });
            }
            existingCategory.Name = category.Name;
            _dbContext.SaveChanges();
            return Ok(existingCategory);
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
            return Ok(new { message = "Category deleted successfully" });
        }
    }
}
