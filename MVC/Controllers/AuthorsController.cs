using AutoMapper;
using LiBaby.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LiBaby.Controllers
{
	public class AuthorsController : Controller
	{
		private readonly IMapper _mapper;
		private readonly HttpClient _httpClient;

		public AuthorsController(IMapper mapper)
		{
			_mapper = mapper;
			_httpClient = HttpClientFactory.Create();
		}

		// GET: Authors
		public async Task<IActionResult> Index()
		{
			var res = await _httpClient.GetAsync("https://localhost:7289/api/Authors/");
			return res.StatusCode == HttpStatusCode.OK
					? View(res.Content.ReadFromJsonAsync<IEnumerable<AuthorViewModel>>().Result)
					: Problem("Entity set 'KpzDbContext.Authors' is null.");
		}

		// GET: Authors/Details/5
		public async Task<IActionResult> Details(int id)
		{
			var res = await _httpClient.GetAsync($"https://localhost:7289/api/Authors/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<AuthorViewModel>().Result)
				: Problem("Entity set 'KpzDbContext.Authors' is null.");
		}

		// GET: Authors/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Authors/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("FirstName,LastName,Birthday,Bio")] AuthorViewModel author)
		{
			var httpResponse = await _httpClient.PostAsync("https://localhost:7289/api/Authors/",
				new StringContent(JsonSerializer.Serialize(author), Encoding.UTF8, "application/json"));
			return RedirectToAction(nameof(Index));
		}

		// GET: Authors/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			var res = await _httpClient.GetAsync($"https://localhost:7289/api/Authors/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<AuthorViewModel>().Result)
				: Problem("Entity set 'KpzDbContext.Authors' is null.");
		}

		// POST: Authors/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,Birthday,Bio")] AuthorViewModel author)
		{
			var httpResponse = await _httpClient.PutAsync($"https://localhost:7289/api/Authors/{id}",
				new StringContent(JsonSerializer.Serialize(author), Encoding.UTF8, "application/json"));
			return RedirectToAction(nameof(Index));
		}

		// GET: Authors/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var res = await _httpClient.GetAsync($"https://" +
												 $"localhost:7289" +
												 $"/api/Authors/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<AuthorViewModel>().Result)
				: Problem("Entity set 'KpzDbContext.Authors' is null.");
		}

		// POST: Authors/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var res = await _httpClient.DeleteAsync($"https://localhost:7289/api/Authors/{id}");
			return RedirectToAction(nameof(Index));
		}
	}
}
