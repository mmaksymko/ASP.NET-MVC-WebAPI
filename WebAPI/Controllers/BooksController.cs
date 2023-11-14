using AutoMapper;
using LiBaby.Models;
using LiBaby.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace lab3_1.Controllers.API
{
	[Route("api/Books")]
	[ApiController]
	public class BooksController : ControllerBase
	{
		private readonly KpzDbContext _context;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public BooksController(KpzDbContext context, ILogger<BooksController> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}

		/// <summary>
		/// Gets all books.
		/// </summary>
		/// <returns>A list of books</returns>
		/// <remarks>
		/// Sample request:
		/// 
		///     GET /api/Books
		/// </remarks>
		/// <response code="200">Returns all items</response>
		/// <response code="204">Items were not found</response>
		/// <response code="400">Invalid values were entered.</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]

		public async Task<ActionResult<string>> GetBooks()
		{
			if (_context.Books == null)
			{
				_logger.LogError("Table \"Book\" was not found.");
				return NotFound();
			}

			_logger.LogInformation("Successfully retrieved data about books.");
			return Ok(JsonSerializer.Serialize(_context.Books.ToList()));
		}

		/// <summary>
		/// Gets an book with a specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Book with a specified id</returns>
		/// <remarks>
		/// Sample request:
		/// 
		///     GET /api/Books/{id}
		/// </remarks>
		/// <response code="200">Returns an item</response>
		/// <response code="204">Item was not found</response>
		/// <response code="404">Invalid values were entered.</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<string>> GetBook(int id)
		{
			if (_context.Books == null)
			{
				_logger.LogError("Table \"Book\" was not found.");
				return NotFound();
			}
			var book = await _context.Books.FindAsync(id);

			if (book == null)
			{
				_logger.LogInformation("Book was not found.");
				return NoContent();
			}

			_logger.LogInformation("Successfully retrieved data about a book.");
			return Ok(JsonSerializer.Serialize(_mapper.Map<BookViewModel>(book)));
		}

		/// <summary>
		/// Updates an book with a specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <remarks>
		/// Sample request:
		/// 
		///     PUT /api/Books/{id}
		///		{
		///			"PublisherId": 1,
		///			"Title": "Kobzar",
		///			"ReleaseYear" : 1840,
		///			"Pages": 114
		///		}
		/// </remarks>
		/// <response code="201">Puts an items</response>
		/// <response code="400">Validation failed</response>
		/// <response code="404">DB table was not found</response>
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> PutBook(int id, [FromBody] BookViewModel book)
		{
			if (!isValid(book))
			{
				_logger.LogInformation("Invalid values were entered.");
				return ValidationProblem("Publisher_Id doesn't exist or Empty values were entered.");
			}

			if (ModelState.IsValid)
			{
				try
				{
					var foundBook = _context.Books.SingleOrDefault(book => book.BookId == id);
					if (foundBook != null)
					{
						foundBook.PublisherId = book.PublisherId;
						foundBook.Pages = book.Pages;
						foundBook.Title = book.Title;
						foundBook.ReleaseYear = book.ReleaseYear;
						_context.SaveChanges();
						_logger.LogInformation("Book was successfully updated.");
						return Created("book", JsonSerializer.Serialize(foundBook));
					}
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!BookExists(book.BookId))
					{
						_logger.LogError("Book was not found.");
						return NotFound();
					}
					else
					{
						_logger.LogError("Error while updating book.");
						return Conflict();
					}
				}
				return BadRequest();
			}
			_logger.LogError("Table \"Book\" was not found.");
			return NotFound();
		}

		/// <summary>
		/// Adds a new book.
		/// </summary>
		/// <remarks>
		/// Sample request:
		/// 
		///     POST /api/Books/{id}
		///		{
		///			"PublisherId": 1,
		///			"Title": "Kobzar",
		///			"ReleaseYear" : 1840,
		///			"Pages": 114
		///		}
		/// </remarks>
		/// <response code="201">Posts an items</response>
		/// <response code="204">Table is empty</response>
		/// <response code="404">DB table was not found</response>
		/// <response code="422">Validation failed</response>
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

		public async Task<ActionResult<BookViewModel>> PostBook([FromBody] BookViewModel book)
		{
			if (_context.Books == null)
			{
				_logger.LogInformation("Table \"Book\" was not found.");
				return NoContent();
			}

			if (!isValid(book))
			{
				_logger.LogInformation("Invalid values were entered.");
				return UnprocessableEntity("Publisher Id doesn't exist or Empty values were entered.");
			}

			var entry = _context.Books.Add(_mapper.Map<Book>(book));
			await _context.SaveChangesAsync();

			_logger.LogInformation("Book was succesfully added.");
			return Created("book", JsonSerializer.Serialize(entry));
		}

		/// <summary>
		/// Deletes an book with specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <remarks>
		/// Sample request:
		/// 
		///     DELETE /api/Books/{id}
		/// </remarks>
		/// <response code="200">Deletes an item</response>
		/// <response code="204">Item was not found</response>
		/// <response code="404">DB table was not found</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteBook(int id)
		{
			if (_context.Books == null)
			{
				_logger.LogInformation("Table \"Book\" was not found.");
				return NotFound();
			}
			var book = await _context.Books.FindAsync(id);

			if (book == null)
			{
				_logger.LogInformation("Book was not found.");
				return NoContent();
			}

			_context.Books.Remove(book);
			await _context.SaveChangesAsync();

			_logger.LogInformation("Book was succesfully deleted!");
			return Ok();
		}

		private bool isValid(BookViewModel book)
			=> !string.IsNullOrWhiteSpace(book.Title) && book.Pages > 0 && book.ReleaseYear > 0 &&
			   _context.Publishers.Where(publisher => publisher.PublisherId == book.PublisherId).Any();

		private bool BookExists(int id)
		{
			return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
		}
	}
}
