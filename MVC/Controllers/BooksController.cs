using AutoMapper;
using LiBaby.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LiBaby.Controllers
{
	public class BooksController : Controller
	{
		private readonly IMapper _mapper;
		private readonly HttpClient _httpClient;

		public BooksController(IMapper mapper)
		{
			_mapper = mapper;
			_httpClient = HttpClientFactory.Create();
		}

		// GET: Books
		public async Task<IActionResult> Index()
		{
			var res = await _httpClient.GetAsync("https://localhost:7289/api/Books/");
			return res.StatusCode == HttpStatusCode.OK
					? View(res.Content.ReadFromJsonAsync<IEnumerable<BookViewModel>>().Result)
					: Problem("Entity set 'KpzDbContext.Books' is null.");
		}

		// GET: Books/Details/5
		public async Task<IActionResult> Details(int id)
		{
			var res = await _httpClient.GetAsync($"https://localhost:7289/api/Books/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<BookViewModel>().Result)
				: Problem("Entity set 'KpzDbContext.Books' is null.");
		}

		// GET: Books/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Books/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("PublisherId,ReleaseYear,Pages,Title")] BookViewModel book)
		{
			var httpResponse = await _httpClient.PostAsync("https://localhost:7289/api/Books/",
				new StringContent(JsonSerializer.Serialize(book), Encoding.UTF8, "application/json"));
			return RedirectToAction(nameof(Index));
		}

		// GET: Books/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			var res = await _httpClient.GetAsync($"https://localhost:7289/api/Books/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<BookViewModel>().Result)
				: Problem("Entity set 'KpzDbContext.Books' is null.");
		}

		// POST: Books/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("PublisherId,ReleaseYear,Pages,Title")] BookViewModel book)
		{
			var httpResponse = await _httpClient.PutAsync($"https://localhost:7289/api/Books/{id}",
				new StringContent(JsonSerializer.Serialize(book), Encoding.UTF8, "application/json"));
			return RedirectToAction(nameof(Index));
		}

		// GET: Books/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var res = await _httpClient.GetAsync($"https://localhost:7289/api/Books/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<BookViewModel>().Result)
				: Problem("Entity set 'KpzDbContext.Books' is null.");
		}

		// POST: Books/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var res = await _httpClient.DeleteAsync($"https://localhost:7289/api/Books/{id}");
			return RedirectToAction(nameof(Index));
		}
	}
}
