using BackendTasks.Data;
using BackendTasks.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackendTasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;

        public BooksController(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        // GET: api/books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            try
            {
                return Ok(_dbcontext.Books.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving books.", error = ex.Message });
            }
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            try
            {
                var book = _dbcontext.Books.FirstOrDefault(b => b.id == id);
                if (book == null)
                {
                    return NotFound(new { message = "The book was not found." });
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the book.", error = ex.Message });
            }
        }

        // POST: api/books
        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid book data.", errors = ModelState });
                }

                if (_dbcontext.Books.Any(b => b.title == book.title))
                {
                    return Conflict(new { message = "The book you tried to add already exists." });
                }

                _dbcontext.Books.Add(book);
                _dbcontext.SaveChanges();
                return CreatedAtAction(nameof(GetBook), new { id = book.id }, new { message = "The book was added successfully.", book });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while adding the book.", error = ex.Message });
            }
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid book data.", errors = ModelState });
                }

                var bookToUpdate = _dbcontext.Books.FirstOrDefault(b => b.id == id);
                if (bookToUpdate == null)
                {
                    return NotFound(new { message = "The book you are trying to update does not exist." });
                }
                if (bookToUpdate.title == book.title && bookToUpdate.author == book.author && bookToUpdate.publishedDate == book.publishedDate)
                {
                    return BadRequest(new { message = "No changes detected." });
                }
                bookToUpdate.title = book.title;
                bookToUpdate.author = book.author;
                bookToUpdate.publishedDate = book.publishedDate;

                _dbcontext.SaveChanges();
                return Ok(new { message = "The book was updated successfully.", book = bookToUpdate });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the book.", error = ex.Message });
            }
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            try
            {
                var book = _dbcontext.Books.FirstOrDefault(b => b.id == id);
                if (book == null)
                {
                    return NotFound(new { message = "The book was not found." });
                }

                _dbcontext.Books.Remove(book);
                _dbcontext.SaveChanges();
                return Ok(new { message = "The book was deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the book.", error = ex.Message });
            }
        }
    }
}
