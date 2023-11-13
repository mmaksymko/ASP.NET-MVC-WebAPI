using AutoMapper;
using LiBaby.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LiBaby.Controllers
{
	public class PublishersController : Controller
	{
		private readonly IMapper _mapper;
		private readonly HttpClient _httpClient;

		public PublishersController(IMapper mapper)
		{
			_mapper = mapper;
			_httpClient = HttpClientFactory.Create();
		}

		// GET: Publishers
		public async Task<IActionResult> Index()
		{
			var res = await _httpClient.GetAsync("https://localhost:7289/api/Publishers/");
			return res.StatusCode == HttpStatusCode.OK
					? View(res.Content.ReadFromJsonAsync<IEnumerable<PublisherViewModel>>().Result)
					: Problem("Entity set 'KpzDbContext.Publishers' is null.");
		}

		// GET: Publishers/Details/5
		public async Task<IActionResult> Details(int id)
		{
			var res = await _httpClient.GetAsync($"https://localhost:7289/api/Publishers/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<PublisherViewModel>().Result)
				: Problem("Entity set 'KpzDbContext.Publishers' is null.");
		}

		// GET: Publishers/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Publishers/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name,Country")] PublisherViewModel publisher)
		{
			var httpResponse = await _httpClient.PostAsync("https://localhost:7289/api/Publishers/",
				new StringContent(JsonSerializer.Serialize(publisher), Encoding.UTF8, "application/json"));
			return RedirectToAction(nameof(Index));
		}

		// GET: Publishers/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			var res = await _httpClient.GetAsync($"https://localhost:7289/api/Publishers/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<PublisherViewModel>().Result)
				: Problem("Entity set 'KpzDbContext.Publishers' is null.");
		}

		// POST: Publishers/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Name,Country")] PublisherViewModel publisher)
		{
			var httpResponse = await _httpClient.PutAsync($"https://localhost:7289/api/Publishers/{id}",
				new StringContent(JsonSerializer.Serialize(publisher), Encoding.UTF8, "application/json"));
			return RedirectToAction(nameof(Index));
		}

		// GET: Publishers/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var res = await _httpClient.GetAsync($"https://" +
												 $"localhost:7289" +
												 $"/api/Publishers/{id}");
			return res.StatusCode == HttpStatusCode.OK
				? View(res.Content.ReadFromJsonAsync<PublisherViewModel>().Result)
				: Problem("Entity set 'KpzDbContext.Publishers' is null.");
		}

		// POST: Publishers/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var res = await _httpClient.DeleteAsync($"https://localhost:7289/api/Publishers/{id}");
			return RedirectToAction(nameof(Index));
		}
	}
}
