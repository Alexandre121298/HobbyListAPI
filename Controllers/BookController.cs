using Microsoft.AspNetCore.Mvc;

namespace HobbyListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        // Exemple temporaire de liste de livres
        private static readonly List<string> books = new List<string>
        {
            "The Hobbit",
            "1984",
            "Dune"
        };

        // GET: api/book
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            return Ok(books);
        }

        // GET: api/book/{id}
        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            if (id < 0 || id >= books.Count)
                return NotFound();

            return Ok(books[id]);
        }

        // POST: api/book
        [HttpPost]
        public IActionResult AddBook([FromBody] string newBook)
        {
            if (string.IsNullOrWhiteSpace(newBook))
                return BadRequest("Le nom du livre est invalide.");

            books.Add(newBook);
            return CreatedAtAction(nameof(GetBook), new { id = books.Count - 1 }, newBook);
        }
    }
}
