using JobPortalMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JobPortalMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("JobPortalApi");
            var response = await client.GetAsync("jobs");

            if (!response.IsSuccessStatusCode)
                return View(new List<JobViewModel>());

            var json = await response.Content.ReadAsStringAsync();
            var jobs = JsonSerializer.Deserialize<List<JobViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(jobs);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = _clientFactory.CreateClient("JobPortalApi");
            var response = await client.GetAsync($"jobs/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var job = JsonSerializer.Deserialize<JobViewModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(job);
        }
    }
}
