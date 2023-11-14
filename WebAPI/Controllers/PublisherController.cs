using AutoMapper;
using LiBaby.Models;
using LiBaby.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace lab3_1.Controllers.API
{
	[Route("api/Publishers")]
	[ApiController]
	public class PublishersController : ControllerBase
	{
		private readonly KpzDbContext _context;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public PublishersController(KpzDbContext context, ILogger<PublishersController> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}

		/// <summary>
		/// Gets all publishers.
		/// </summary>
		/// <returns>A list of publishers</returns>
		/// <remarks>
		/// Sample request:
		/// 
		///     GET /api/Publishers
		/// </remarks>

		/// <response code="200">Returns all items</response>
		/// <response code="204">Items were not found</response>
		/// <response code="404">DB table was not found</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]

		public async Task<ActionResult<string>> GetPublishers()
		{
			if (_context.Publishers == null)
			{
				_logger.LogError("Table \"Publisher\" was not found.");
				return NotFound();
			}

			_logger.LogInformation("Successfully retrieved data about publishers.");
			return Ok(JsonSerializer.Serialize(_context.Publishers.ToList()));
		}

		/// <summary>
		/// Gets an publisher with a specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Publisher with a specified id</returns>
		/// <remarks>
		/// Sample request:
		/// 
		///     GET /api/Publishers/{id}
		/// </remarks>
		/// <response code="200">Returns an item</response>
		/// <response code="204">Item was not found</response>
		/// <response code="404">DB table was not found</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<string>> GetPublisher(int id)
		{
			if (_context.Publishers == null)
			{
				_logger.LogError("Table \"Publisher\" was not found.");
				return NotFound();
			}
			var publisher = await _context.Publishers.FindAsync(id);

			if (publisher == null)
			{
				_logger.LogInformation("Publisher was not found.");
				return NoContent();
			}

			_logger.LogInformation("Successfully retrieved data about a publisher.");
			return Ok(JsonSerializer.Serialize(_mapper.Map<PublisherViewModel>(publisher)));
		}

		/// <summary>
		/// Updates an publisher with a specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <remarks>
		/// Sample request:
		/// 
		///     PUT /api/Publishers/{id}
		///		{
		///			"Country": "Ukraine",
		///			"Name": "Vydavnytstvo"
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
		public async Task<IActionResult> PutPublisher(int id, [FromBody] PublisherViewModel publisher)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (!isValid(publisher))
					{
						_logger.LogInformation("Invalid values were entered.");
						return ValidationProblem("Empty values were entered.");
					}
					var foundPublisher = _context.Publishers.SingleOrDefault(publisher => publisher.PublisherId == id);
					if (foundPublisher != null)
					{
						foundPublisher.Country = publisher.Country;
						foundPublisher.Name = publisher.Name;
						_context.SaveChanges();
						_logger.LogInformation("Publisher was successfully updated.");
						return Created("publisher", JsonSerializer.Serialize(foundPublisher));
					}
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PublisherExists(publisher.PublisherId))
					{
						_logger.LogError("Publisher was not found.");
						return NotFound();
					}
					else
					{
						_logger.LogError("Error while updating publisher.");
						return Conflict();
					}
				}
				return BadRequest();
			}
			_logger.LogError("Table \"Publisher\" was not found.");
			return NotFound();
		}

		/// <summary>
		/// Adds a new publisher.
		/// </summary>
		/// <remarks>
		/// Sample request:
		/// 
		///     POST /api/Publishers/
		///		{
		///			"Country": "Ukraine",
		///			"Name": "Vydavnytstvo"
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

		public async Task<ActionResult<PublisherViewModel>> PostPublisher([FromBody] PublisherViewModel publisher)
		{
			if (_context.Publishers == null)
			{
				_logger.LogInformation("Table \"Publisher\" was not found.");
				return NoContent();
			}

			if (!isValid(publisher))
			{
				_logger.LogInformation("Invalid values were entered.");
				return UnprocessableEntity("Empty values were entered.");
			}

			var entry = _context.Publishers.Add(_mapper.Map<Publisher>(publisher));
			await _context.SaveChangesAsync();

			_logger.LogInformation("Publisher was succesfully added.");
			return Created("publisher", JsonSerializer.Serialize(entry));
		}

		/// <summary>
		/// Deletes an publisher with specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <remarks>
		/// Sample request:
		/// 
		///     DELETE /api/Publishers/{id}
		/// </remarks>
		/// <response code="200">Deletes an item</response>
		/// <response code="204">Item was not found</response>
		/// <response code="404">DB table was not found</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeletePublisher(int id)
		{
			if (_context.Publishers == null)
			{
				_logger.LogInformation("Table \"Publisher\" was not found.");
				return NotFound();
			}
			var publisher = await _context.Publishers.FindAsync(id);

			if (publisher == null)
			{
				_logger.LogInformation("Publisher was not found.");
				return NoContent();
			}

			_context.Publishers.Remove(publisher);
			await _context.SaveChangesAsync();

			_logger.LogInformation("Publisher was succesfully deleted.");
			return Ok();
		}

		private bool isValid(PublisherViewModel publisher)
			=> !string.IsNullOrWhiteSpace(publisher.Country) && !string.IsNullOrWhiteSpace(publisher.Name);

		private bool PublisherExists(int id)
		{
			return (_context.Publishers?.Any(e => e.PublisherId == id)).GetValueOrDefault();
		}
	}
}
