using AutoMapper;
using LiBaby.Models;
using LiBaby.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LiBaby.Controllers.API
{
	[Route("api/Authors")]
	[ApiController]
	public class AuthorsController : ControllerBase
	{
		private readonly KpzDbContext _context;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public AuthorsController(KpzDbContext context, ILogger<AuthorsController> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}

		/// <summary>
		/// Gets all authors.
		/// </summary>
		/// <returns>A list of authors</returns>
		/// <remarks>
		/// Sample request:
		/// 
		///     GET /api/Authors
		/// </remarks>
		/// <response code="200">Returns all items</response>
		/// <response code="204">Items were not found</response>
		/// <response code="400">DB table was not found</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]

		public async Task<ActionResult<string>> GetAuthors()
		{
			if (_context.Authors == null || _context.People == null)
			{
				_logger.LogError("Table \"Author\" or \"People\" was not found.");
				return NotFound();
			}

			_logger.LogInformation("Successfully retrieved data about authors.");
			return Ok(JsonSerializer.Serialize(_context.Authors.ToList().Select(author => _mapper.Map<AuthorViewModel>((author,
					_context.People.Where(person => person.PersonId == author.AuthorId).First()))).ToList()));
		}

		/// <summary>
		/// Gets an author with a specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Author with a specified id</returns>
		/// <remarks>
		/// Sample request:
		/// 
		///     GET /api/Authors/{id}
		/// </remarks>
		/// <response code="200">Returns an item</response>
		/// <response code="204">Item was not found</response>
		/// <response code="404">DB table was not found</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<string>> GetAuthor(int id)
		{
			if (_context.Authors == null || _context.People == null)
			{
				_logger.LogError("Table \"Author\" or \"People\" was not found.");
				return NotFound();
			}
			var author = await _context.Authors.FindAsync(id);
			var person = await _context.People.FindAsync(id);

			if (author == null || person == null)
			{
				_logger.LogInformation("Author was not found.");
				return NoContent();
			}

			_logger.LogInformation("Successfully retrieved data about an author.");
			return Ok(JsonSerializer.Serialize(_mapper.Map<AuthorViewModel>((author, person))));
		}

		/// <summary>
		/// Updates an author with a specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <remarks>
		/// Sample request:
		/// 
		///     PUT /api/Authors/{id}
		///		{
		///			"FirstName": "Bohdan",
		///			"LastName": "Melnyk",
		///			"Birthday" : "1922-01-01",
		///			"Bio": "He was born when he was born and died when he died"
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
		public async Task<IActionResult> PutAuthor(int id, [FromBody] AuthorViewModel author)
		{
			if (!isValid(author))
			{
				_logger.LogInformation("Invalid values were entered.");
				return ValidationProblem("Empty values were entered.");
			}
			if (ModelState.IsValid)
			{
				try
				{
					var foundPerson = _context.People.SingleOrDefault(person => person.PersonId == id);
					var foundAuthor = _context.Authors.SingleOrDefault(author => author.AuthorId == id);
					if (foundPerson != null && foundAuthor != null)
					{
						foundAuthor.Bio = author.Bio;
						foundPerson.FirstName = author.FirstName;
						foundPerson.LastName = author.LastName;
						foundPerson.Birthday = author.Birthday;
						_context.SaveChanges();
						_logger.LogInformation("Author was successfully updated.");
						return Created("author", JsonSerializer.Serialize(_mapper.Map<AuthorViewModel>((foundAuthor, foundPerson))));
					}
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!AuthorExists(author.AuthorId))
					{
						_logger.LogError("Author was not found.");
						return NotFound();
					}
					else
					{
						_logger.LogError("Error while updating author.");
						throw;
					}
				}
				return BadRequest();
			}
			_logger.LogError("Table \"Author\" or \"People\" was not found.");
			return NotFound();
		}


		/// <summary>
		/// Adds a new author.
		/// </summary>
		/// <remarks>
		/// Sample request:
		/// 
		///     POST /api/Authors/
		///		{
		///			"FirstName": "Bohdan",
		///			"LastName": "Melnyk",
		///			"Birthday" : "1922-01-01",
		///			"Bio": "He was born when he was born and died when he died"
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
		public async Task<ActionResult<AuthorViewModel>> PostAuthor([FromBody] AuthorViewModel author)
		{
			if (_context.Authors == null || _context.People == null)
			{
				_logger.LogError("Table \"Author\" or \"People\" was not found.");
				return NoContent();
			}
			if (!isValid(author))
			{
				_logger.LogInformation("Invalid values were entered.");
				return UnprocessableEntity("Empty values were entered.");
			}

			var personEntry = _context.People.Add(_mapper.Map<Person>(author));
			var _author = _mapper.Map<Author>(author);
			_author.AuthorId = personEntry.Entity.PersonId;
			var authorEntry = _context.Authors.Add(_author);
			await _context.SaveChangesAsync();

			_logger.LogInformation("Author was succesfully added.");
			return Created("author", JsonSerializer.Serialize(_mapper.Map<AuthorViewModel>((authorEntry.Entity, personEntry.Entity))));
		}

		/// <summary>
		/// Deletes an author with specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <remarks>
		/// Sample request:
		/// 
		///     DELETE /api/Authors/{id}
		/// </remarks>
		/// <response code="200">Deletes an item</response>
		/// <response code="204">Item was not found</response>
		/// <response code="404">DB table was not found</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]

		public async Task<IActionResult> DeleteAuthor(int id)
		{
			if (_context.Authors == null || _context.People == null)
			{
				_logger.LogError("Table \"Author\" or \"People\" was not found.");
				return NotFound();
			}
			var author = await _context.Authors.FindAsync(id);
			var person = await _context.People.FindAsync(id);

			if (author == null || person == null)
			{
				_logger.LogInformation("Author was not found.");
				return NoContent();
			}

			_context.Authors.Remove(author);
			_context.People.Remove(person);
			await _context.SaveChangesAsync();

			_logger.LogInformation("Author was successfully deleted.");
			return Ok();
		}

		private bool isValid(AuthorViewModel author)
			=> !string.IsNullOrWhiteSpace(author.FirstName) && !string.IsNullOrWhiteSpace(author.LastName) && !string.IsNullOrWhiteSpace(author.Bio);

		private bool AuthorExists(int id)
		{
			return (_context.Authors?.Any(e => e.AuthorId == id)).GetValueOrDefault();
		}
	}
}
