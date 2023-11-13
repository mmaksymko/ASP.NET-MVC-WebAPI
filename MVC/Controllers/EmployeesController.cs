using AutoMapper;
using LiBaby.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LiBaby.Controllers
{
	public class EmployeesController : Controller
	{
		private readonly IMapper _mapper;
		private readonly HttpClient _httpClient;

		public EmployeesController(IMapper mapper)
		{
			_mapper = mapper;
			_httpClient = HttpClientFactory.Create();
		}

		// GET: Employees
		public async Task<IActionResult> Index()
		{
			var res = await _httpClient.GetAsync("https://localhost:7289/api/Employees/");
			return res.StatusCode == HttpStatusCode.OK
					? View(res.Content.ReadFromJsonAsync<IEnumerable<EmployeeViewModel>>().Result)
					: Problem("Entity set 'KpzDbContext.Employees' is null.");
		}

		// GET: Employees/Details/5
		public async Task<IActionResult> Details(int id)
		{
			var res = await _httpClient.GetAsync($"https://localhost:7289/api/Employees/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<EmployeeViewModel>().Result)
				: Problem("Entity set 'KpzDbContext.Employees' is null.");
		}

		// GET: Employees/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Employees/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("FirstName,LastName,Birthday,Salary")] EmployeeViewModel employee)
		{
			var httpResponse = await _httpClient.PostAsync("https://localhost:7289/api/Employees/",
				new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json"));
			return RedirectToAction(nameof(Index));
		}

		// GET: Employees/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			var res = await _httpClient.GetAsync($"https://localhost:7289/api/Employees/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<EmployeeViewModel>().Result)
				: Problem("Entity set 'KpzDbContext.Employees' is null.");
		}

		// POST: Employees/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,Birthday,Salary")] EmployeeViewModel employee)
		{
			var httpResponse = await _httpClient.PutAsync($"https://localhost:7289/api/Employees/{id}",
				new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json"));
			return RedirectToAction(nameof(Index));
		}

		// GET: Employees/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var res = await _httpClient.GetAsync($"https://" +
												 $"localhost:7289" +
												 $"/api/Employees/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<EmployeeViewModel>().Result)
				: Problem("Entity set 'KpzDbContext.Employees' is null.");
		}

		// POST: Employees/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var res = await _httpClient.DeleteAsync($"https://localhost:7289/api/Employees/{id}");
			return RedirectToAction(nameof(Index));
		}
	}
}
