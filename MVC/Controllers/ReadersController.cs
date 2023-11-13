using AutoMapper;
using LiBaby.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LiBaby.Controllers
{
	public class ReadersController : Controller
	{
		private readonly IMapper _mapper;
		private readonly HttpClient _httpClient;

		public ReadersController(IMapper mapper)
		{
			_mapper = mapper;
			_httpClient = HttpClientFactory.Create();
		}

		// GET: Readers
		public async Task<IActionResult> Index()
		{
			var res = await _httpClient.GetAsync("https://localhost:7289/api/Readers/");
			return res.StatusCode == HttpStatusCode.OK
					? View(res.Content.ReadFromJsonAsync<IEnumerable<ReaderViewModel>>().Result)
					: Problem("Entity set 'KpzDbContext.Readers' is null.");
		}

		// GET: Readers/Details/5
		public async Task<IActionResult> Details(int id)
		{
			var res = await _httpClient.GetAsync($"https://localhost:7289/api/Readers/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<ReaderViewModel>().Result)
				: RedirectToAction(nameof(Index));
		}

		// GET: Readers/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Readers/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Address,Birthday")] ReaderViewModel reader)
		{
			var httpResponse = await _httpClient.PostAsync("https://localhost:7289/api/Readers/",
				new StringContent(JsonSerializer.Serialize(reader), Encoding.UTF8, "application/json"));
			return RedirectToAction(nameof(Index));
		}

		// GET: Readers/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			var res = await _httpClient.GetAsync($"https://localhost:7289/api/Readers/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<ReaderViewModel>().Result)
				: Problem("Entity set 'KpzDbContext.Readers' is null.");
		}

		// POST: Readers/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,Email,Address,Birthday")] ReaderViewModel reader)
		{
			var httpResponse = await _httpClient.PutAsync($"https://localhost:7289/api/Readers/{id}",
				new StringContent(JsonSerializer.Serialize(reader), Encoding.UTF8, "application/json"));
			return RedirectToAction(nameof(Index));
		}

		// GET: Readers/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var res = await _httpClient.GetAsync($"https://" +
												 $"localhost:7289" +
												 $"/api/Readers/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<ReaderViewModel>().Result)
				: Problem("Entity set 'KpzDbContext.Readers' is null.");
		}

		// POST: Readers/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var res = await _httpClient.DeleteAsync($"https://localhost:7289/api/Readers/{id}");
			return RedirectToAction(nameof(Index));
		}
	}
}
