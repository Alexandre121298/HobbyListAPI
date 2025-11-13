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
        private readonly ILogger<BookController> _logger;

        public BookController(AppDbContext context, ILogger<BookController> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region GET
        // GET: api/book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            _logger.LogInformation("Récupération de tous les livres");
            var books = await _context.Books.ToListAsync();
            return Ok(books);
        }

        // GET: api/book/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                _logger.LogWarning("Livre introuvable pour l'ID {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Livre récupéré : {Title}", book.Title);
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
            {
                _logger.LogWarning("Tentative d'ajout d'un livre invalide");
                return BadRequest(ModelState);
            }
                

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Livre ajouté : {Title}", newBook.Title);
            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }

        // PUT: api/book/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book updatedBook)
        {
            if (id != updatedBook.Id)
            {
                _logger.LogWarning("Tentative de mise à jour d'un livre avec ID incohérent {Id}", id);
                return BadRequest("L'ID du livre ne correspond pas.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Tentative de mise à jour d'un livre invalide avec ID {Id}", id);
                return BadRequest(ModelState);
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                _logger.LogWarning("Livre introuvable pour la mise à jour avec ID {Id}", id);
                return NotFound();
            }

            book.Title = updatedBook.Title;
            book.PurchaseDate = updatedBook.PurchaseDate;
            book.Price = updatedBook.Price;
            book.Status = updatedBook.Status;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Livre mis à jour : {Title}", book.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du livre avec ID {Id}", id);
                throw; // sera capturé par le middleware global
            }

            return NoContent();
        }



        // DELETE: api/book/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                _logger.LogWarning("Tentative de suppression d'un livre inexistant avec ID {Id}", id);
                return NotFound();
            }

            _context.Books.Remove(book);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Livre supprimé : {Title}", book.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du livre avec ID {Id}", id);
                throw; // sera capturé par le middleware global
            }

            return NoContent();
        }
        #endregion
    }
}
