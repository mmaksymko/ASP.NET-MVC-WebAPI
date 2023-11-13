using AutoMapper;
using LiBaby.Models;
using LiBaby.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace lab3_1.Controllers.API
{
	[Route("api/Employees")]
	[ApiController]
	public class EmployeesController : ControllerBase
	{
		private readonly KpzDbContext _context;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public EmployeesController(KpzDbContext context, ILogger<EmployeesController> logger, IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}

		/// <summary>
		/// Gets all employees.
		/// </summary>
		/// <returns>A list of employees</returns>
		/// <remarks>
		/// Sample request:
		/// 
		///     GET /api/Employees
		/// </remarks>
		/// <response code="200">Returns all items</response>
		/// <response code="204">Item was not found</response>
		/// <response code="400">Invalid values were entered.</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<string>> GetEmployees()
		{
			if (_context.Employees == null || _context.People == null)
			{
				_logger.LogError("Table \"Employee\" or \"Person\" was not found.");
				return NotFound();
			}

			_logger.LogInformation("Successfully retrieved data about employees.");
			return Ok(JsonSerializer.Serialize(_context.Employees.ToList().Select(employee => _mapper.Map<EmployeeViewModel>((employee,
					_context.People.Where(person => person.PersonId == employee.EmployeeId).First()))).ToList()));
		}

		/// <summary>
		/// Gets an employee with a specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Employee with a specified id</returns>
		/// <remarks>
		/// Sample request:
		/// 
		///     GET /api/Employees/{id}
		/// </remarks>
		/// <response code="200">Returns an item</response>
		/// <response code="204">Items were not found</response>
		/// <response code="404">Invalid values were entered.</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<string>> GetEmployee(int id)
		{
			if (_context.Employees == null || _context.People == null)
			{
				_logger.LogError("Table \"Employee\" or \"Person\" was not found.");
				return NotFound();
			}
			var employee = await _context.Employees.FindAsync(id);
			var person = await _context.People.FindAsync(id);

			if (employee == null || person == null)
			{
				_logger.LogInformation("Employee was not found.");
				return NoContent();
			}

			_logger.LogInformation("Successfully retrieved data about an employee.");
			return Ok(JsonSerializer.Serialize(_mapper.Map<EmployeeViewModel>((employee, person))));
		}


		/// <summary>
		/// Updates an employee with a specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <remarks>
		/// Sample request:
		/// 
		///     PUT /api/Employees/{id}
		///		{
		///			"FirstName": "Bohdan",
		///			"LastName": "Melnyk",
		///			"Birthday" : "1922-01-01",
		///			"Salary": 120000
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

		public async Task<IActionResult> PutEmployee(int id, [FromBody] EmployeeViewModel employee)
		{
			if (!isValid(employee))
			{
				_logger.LogInformation("Invalid values were entered.");
				return ValidationProblem("Empty values were entered.");
			}

			if (ModelState.IsValid)
			{
				try
				{
					var foundPerson = _context.People.SingleOrDefault(person => person.PersonId == id);
					var foundemployee = _context.Employees.SingleOrDefault(employee => employee.EmployeeId == id);
					if (foundPerson != null && foundemployee != null)
					{
						foundemployee.Salary = employee.Salary;
						foundPerson.FirstName = employee.FirstName;
						foundPerson.LastName = employee.LastName;
						foundPerson.Birthday = employee.Birthday;
						_context.SaveChanges();
						_logger.LogInformation("Employee was successfully updated.");
						return Created("employee", employee);
					}
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!employeeExists(employee.EmployeeId))
					{
						_logger.LogError("Employee was not found.");
						return NotFound();
					}
					else
					{
						_logger.LogError("Error while updating.");
						throw;
					}
				}
				return BadRequest();
			}
			_logger.LogError("Table \"Employee\" or \"Person\" was not found.");
			return NotFound();
		}

		/// <summary>
		/// Adds a new employee.
		/// </summary>
		/// <param name="id"></param>
		/// <remarks>
		/// Sample request:
		/// 
		///     POST /api/Employees/{id}
		///		{
		///			"FirstName": "Bohdan",
		///			"LastName": "Melnyk",
		///			"Birthday" : "1922-01-01",
		///			"Salary": 120000
		///		}
		/// </remarks>
		/// <response code="201">Posts an items</response>
		/// <response code="400">Validation failed</response>
		/// <response code="404">DB table was not found</response>
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<EmployeeViewModel>> PostEmployee([FromBody] EmployeeViewModel employee)
		{
			if (_context.Employees == null)
			{
				_logger.LogInformation("Table \"Employee\" or \"Person\" was not found.");
				return Problem("Table is null.");
			}
			if (!isValid(employee))
			{
				_logger.LogInformation("Invalid values were entered.");
				return ValidationProblem("Empty values were entered.");
			}

			_context.People.Add(_mapper.Map<Person>(employee));
			await _context.SaveChangesAsync();

			var _employee = _mapper.Map<Employee>(employee);
			_employee.EmployeeId = _context.People.Max(p => p.PersonId);
			_context.Employees.Add(_employee);
			await _context.SaveChangesAsync();

			_logger.LogInformation("Employee was succesfully added.");
			return Created("employee", employee);
		}

		/// <summary>
		/// Deletes an employee with specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <remarks>
		/// Sample request:
		/// 
		///     DELETE /api/Employees/{id}
		/// </remarks>
		/// <response code="200">Deletes an item</response>
		/// <response code="204">Item was not found</response>
		/// <response code="404">DB table was not found</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteEmployee(int id)
		{
			if (_context.Employees == null || _context.People == null)
			{
				_logger.LogInformation("Table \"Employee\" or \"Person\" was not found.");
				return NotFound();
			}
			var employee = await _context.Employees.FindAsync(id);
			var person = await _context.People.FindAsync(id);

			if (employee == null || person == null)
			{
				_logger.LogInformation("Employee was not found.");
				return NoContent();
			}

			_context.Employees.Remove(employee);
			_context.People.Remove(person);
			await _context.SaveChangesAsync();

			_logger.LogInformation("Employee was succesfully deleted.");
			return Ok();
		}

		private bool isValid(EmployeeViewModel employee)
			=> !string.IsNullOrWhiteSpace(employee.FirstName) && !string.IsNullOrWhiteSpace(employee.LastName) && employee.Salary > 0;


		private bool employeeExists(int id)
		{
			return (_context.Employees?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
		}
	}
}
