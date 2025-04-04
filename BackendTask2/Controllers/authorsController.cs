using BackendTask2.Model;
using BackendTasks.Data;
using BackendTasks.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendTask2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;

        public AuthorsController(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Author>> GetAuthors()
        {
            var authors = _dbcontext.Authors.ToList();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public ActionResult GetAuthor(int id)
        {
            var author = _dbcontext.Authors.FirstOrDefault(a => a.ID == id);
            if (author == null)
                return NotFound(new { message = "Author not found" });

            return Ok(new { author.Name, author.BirthDate });
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] AuthorDto authorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newAuthor = new Author
            {
                Name = authorDto.Name,
                BirthDate = authorDto.BirthDate
            };

            _dbcontext.Authors.Add(newAuthor);
            _dbcontext.SaveChanges();

            return CreatedAtAction(nameof(GetAuthor), new { id = newAuthor.ID }, new { newAuthor.Name, newAuthor.BirthDate });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAuthor(int id, [FromBody] AuthorDto updateAuthor)
        {
            var author = _dbcontext.Authors.FirstOrDefault(a => a.ID == id);
            if (author == null)
                return NotFound(new { message = "Author not found" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (author.Name == updateAuthor.Name && author.BirthDate == updateAuthor.BirthDate)
                return BadRequest(new { message = "No changes were made" });

            author.Name = updateAuthor.Name;
            author.BirthDate = updateAuthor.BirthDate;
            _dbcontext.SaveChanges();

            return Ok(new { message = $"The Author {author.Name} has been updated" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            var author = _dbcontext.Authors.FirstOrDefault(a => a.ID == id);
            if (author == null)
                return NotFound(new { message = "Author not found" });

            _dbcontext.Authors.Remove(author);
            _dbcontext.SaveChanges();

            return Ok(new { message = $"The Author {author.Name} has been deleted" });
        }

        [HttpGet("{AuthorId}/books")]
        public ActionResult<IEnumerable<Book>> GetBooksByAuthor(int AuthorId)
        {
            var author = _dbcontext.Authors.FirstOrDefault(a => a.ID == AuthorId);
            if (author == null)
                return NotFound(new { message = "Author not found" });

            var books = _dbcontext.Books.Where(x => x.AuthorID == AuthorId)
                                        .Select(x => new { x.Title, x.PublishDate })
                                        .ToList();

            return Ok(books);
        }

        [HttpPost("{AuthorId}/books")]
        public IActionResult AddBookToAuthor(int AuthorId, [FromBody] BookDto bookDto)
        {
            var author = _dbcontext.Authors.FirstOrDefault(a => a.ID == AuthorId);
            if (author == null)
                return NotFound(new { message = "Author not found" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = new Book
            {
                Title = bookDto.Title,
                PublishDate = bookDto.PublishDate,
                Price = bookDto.Price,
                AuthorID = AuthorId
            };

            _dbcontext.Books.Add(book);
            _dbcontext.SaveChanges();

            return Ok(new { message = $"The book '{book.Title}' has been added to the author {author.Name}" });
        }

        [HttpGet("{AuthorId}/books/{Id}")]
        public IActionResult GetBookByIdAndAuthor(int AuthorId, int Id)
        {
            var book = _dbcontext.Books.FirstOrDefault(x => x.AuthorID == AuthorId && x.ID == Id);
            if (book == null)
                return NotFound(new { message = "Book not found" });

            return Ok(new { book.Title, book.PublishDate });
        }

        [HttpPut("{AuthorId}/books/{Id}")]
        public IActionResult UpdateBookByAuthor(int AuthorId, int Id, [FromBody] BookDto updateBook)
        {
            var book = _dbcontext.Books.FirstOrDefault(x => x.AuthorID == AuthorId && x.ID == Id);
            if (book == null)
                return NotFound(new { message = "Book not found" });

            if (updateBook == null || string.IsNullOrEmpty(updateBook.Title) || updateBook.PublishDate == default)
                return BadRequest(new { message = "Invalid book data. Title and PublishDate are required." });

            if (book.Title == updateBook.Title && book.PublishDate == updateBook.PublishDate)
                return BadRequest(new { message = "No changes were made." });

            book.Title = updateBook.Title;
            book.PublishDate = updateBook.PublishDate;
            book.Price = updateBook.Price;
            _dbcontext.Books.Update(book);
            _dbcontext.SaveChanges();

            return Ok(new { message = "The book has been updated successfully.", book = new { book.Title, book.Price, book.PublishDate } });
        }

        [HttpDelete("{AuthorId}/books/{Id}")]
        public IActionResult DeleteBookByIdAndAuthor(int AuthorId, int Id)
        {
            var book = _dbcontext.Books.FirstOrDefault(x => x.AuthorID == AuthorId && x.ID == Id);
            if (book == null)
                return NotFound(new { message = "Book not found" });

            _dbcontext.Books.Remove(book);
            _dbcontext.SaveChanges();

            return Ok(new { message = "The book has been deleted" });
        }
    }
}
