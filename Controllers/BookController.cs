using HobbyListAPI.Data;
using HobbyListAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HobbyListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        #region GET
        // GET: api/book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var books = await _context.Books.ToListAsync();
            return Ok(books);
        }

        // GET: api/book/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return NotFound();

            return Ok(book);
        }


        // GET: api/book/sorted
        [HttpGet("sorted")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksSortedByDate(bool ascending = false)
        {
            var books = ascending
                ? await _context.Books.OrderBy(b => b.PurchaseDate).ToListAsync()
                : await _context.Books.OrderByDescending(b => b.PurchaseDate).ToListAsync();

            return Ok(books);
        }
        #endregion

        #region POST/PUT/DELETE
        // POST: api/book
        [HttpPost]
        public async Task<ActionResult<Book>> AddBook([FromBody] Book newBook)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }

        // PUT: api/book/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book updatedBook)
        {
            if (id != updatedBook.Id)
                return BadRequest("L'ID du livre ne correspond pas.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            book.Title = updatedBook.Title;
            book.PurchaseDate = updatedBook.PurchaseDate;
            book.Price = updatedBook.Price;
            book.Status = updatedBook.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }



        // DELETE: api/book/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
