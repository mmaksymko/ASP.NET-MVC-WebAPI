using AutoMapper;
using LiBaby.Models;
using LiBaby.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace lab3_1.Controllers.API
{
	[Route("api/Readers")]
	[ApiController]
	public class ReadersController : ControllerBase
	{
		private readonly KpzDbContext _context;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public ReadersController(KpzDbContext context, ILogger<ReadersController> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}

		/// <summary>
		/// Gets all readers.
		/// </summary>
		/// <returns>A list of readers</returns>
		/// <remarks>
		/// Sample request:
		/// 
		///     GET /api/Readers
		/// </remarks>
		/// <response code="200">Returns all items</response>
		/// <response code="204">Items were not found</response>
		/// <response code="400">DB table was not found</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<string>> GetReaders()
		{
			if (_context.Readers == null || _context.People == null)
			{
				_logger.LogError("Table \"Reader\" or \"Person\" was not found.");
				return NotFound();
			}

			_logger.LogInformation("Successfully retrieved data about readers.");
			return Ok(JsonSerializer.Serialize(_context.Readers.ToList().Select(reader => _mapper.Map<ReaderViewModel>((reader,
					_context.People.Where(person => person.PersonId == reader.ReaderId).First()))).ToList()));
		}

		/// <summary>
		/// Gets a reader with a specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Reader with a specified id</returns>
		/// <remarks>
		/// Sample request:
		/// 
		///     GET /api/Readers/{id}
		/// </remarks>
		/// <response code="200">Returns an item</response>
		/// <response code="204">Item was not found</response>
		/// <response code="404">DB table was not found</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<string>> GetReader(int id)
		{
			if (_context.Readers == null || _context.People == null)
			{
				_logger.LogError("Table \"Reader\" or \"Person\" was not found.");
				return NotFound();
			}
			var reader = await _context.Readers.FindAsync(id);
			var person = await _context.People.FindAsync(id);

			if (reader == null || person == null)
			{
				_logger.LogInformation("Reader was not found.");
				return NoContent();
			}

			_logger.LogInformation("Successfully retrieved data about a reader.");
			return Ok(JsonSerializer.Serialize(_mapper.Map<ReaderViewModel>((reader, person))));
		}

		/// <summary>
		/// Updates a reader with a specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <remarks>
		/// Sample request:
		/// 
		///     PUT /api/Readers/{id}
		///		{
		///			"FirstName": "Bohdan",
		///			"LastName": "Melnyk",
		///			"Birthday" : "1922-01-01",
		///			"Email" : "bohdan.melnyk.pm.2023@lpnu.ua",
		///			"Address" : "Horodotska, 12"
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
		public async Task<IActionResult> PutReader(int id, [FromBody] ReaderViewModel reader)
		{
			if (!isValid(reader))
			{
				_logger.LogInformation("Invalid values were entered.");
				return ValidationProblem("Email isn't valid or empty values were entered.");
			}
			if (ModelState.IsValid)
			{
				try
				{
					var foundPerson = _context.People.SingleOrDefault(person => person.PersonId == id);
					var foundReader = _context.Readers.SingleOrDefault(reader => reader.ReaderId == id);
					if (foundPerson != null && foundReader != null)
					{
						foundReader.Email = reader.Email;
						foundReader.Address = reader.Address;
						foundPerson.FirstName = reader.FirstName;
						foundPerson.LastName = reader.LastName;
						foundPerson.Birthday = reader.Birthday;
						_context.SaveChanges();
						_logger.LogInformation("Reader was successfully updated.");
						return Created("reader", reader);
					}
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ReaderExists(reader.ReaderId))
					{
						_logger.LogError("Reader was not found.");
						return NotFound();
					}
					else
					{
						_logger.LogError("Error while updating reader.");
						throw;
					}
				}
				return BadRequest();
			}
			_logger.LogError("Table \"Reader\" or \"Person\" was not found.");
			return NotFound();
		}

		/// <summary>
		/// Adds a new reader.
		/// </summary>
		/// <remarks>
		/// Sample request:
		/// 
		///     POST /api/Readers/
		///		{
		///			"FirstName": "Bohdan",
		///			"LastName": "Melnyk",
		///			"Birthday" : "1922-01-01",
		///			"Email" : "bohdan.melnyk.pm.2023@lpnu.ua",
		///			"Address" : "Horodotska, 12"
		///		}
		/// </remarks>
		/// <response code="201">Posts an items</response>
		/// <response code="400">Validation failed</response>
		/// <response code="404">DB table was not found</response>
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ReaderViewModel>> PostReader([FromBody] ReaderViewModel reader)
		{
			if (_context.Readers == null)
			{
				_logger.LogInformation("Table \"Reader\" or \"Person\" was not found.");
				return Problem("Table is null.");
			}

			if (!isValid(reader))
			{
				_logger.LogInformation("Invalid values were entered.");
				return ValidationProblem("Email isn't valid or empty values were entered.");
			}

			_context.People.Add(_mapper.Map<Person>(reader));
			await _context.SaveChangesAsync();

			var _reader = _mapper.Map<Reader>(reader);
			_reader.ReaderId = _context.People.Max(p => p.PersonId);
			_context.Readers.Add(_reader);
			await _context.SaveChangesAsync();

			_logger.LogInformation("Reader was succesfully added.");
			return Created("reader", reader);
		}

		/// <summary>
		/// Deletes a reader with specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <remarks>
		/// Sample request:
		/// 
		///     DELETE /api/Readers/{id}
		/// </remarks>
		/// <response code="200">Deletes an item</response>
		/// <response code="204">Item was not found</response>
		/// <response code="404">DB table was not found</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteReader(int id)
		{
			if (_context.Readers == null || _context.People == null)
			{
				_logger.LogInformation("Table \"Reader\" or \"Person\" was not found.");
				return NotFound();
			}
			var reader = await _context.Readers.FindAsync(id);
			var person = await _context.People.FindAsync(id);

			if (reader == null || person == null)
			{
				_logger.LogInformation("Reader was not found.");
				return NoContent();
			}

			_context.Readers.Remove(reader);
			_context.People.Remove(person);
			await _context.SaveChangesAsync();

			_logger.LogInformation("Reader was succesfully deleted.");
			return Ok();
		}

		private bool isValid(ReaderViewModel reader)
		=> Regex.IsMatch(reader.Email, @"^[a-zA-Z0-9][a-zA-Z0-9.!#$%&\'*+-/=?^_`{|}~]*?[a-zA-Z0-9._-]?@[a-zA-Z0-9][a-zA-Z0-9._-]*?[a-zA-Z0-9]?\.[a-zA-Z]{2,63}$")
		   && !string.IsNullOrWhiteSpace(reader.Address) && !string.IsNullOrWhiteSpace(reader.Email) && !string.IsNullOrWhiteSpace(reader.FirstName) && !string.IsNullOrWhiteSpace(reader.LastName);

		private bool ReaderExists(int id)
		{
			return (_context.Readers?.Any(e => e.ReaderId == id)).GetValueOrDefault();
		}
	}
}
