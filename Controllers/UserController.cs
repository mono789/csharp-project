using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica2.Models;

namespace PruebaTecnica2.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController(IHttpClientFactory httpFactory)
        {
            _httpClient = httpFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {
            string apiUrl= $"https://jsonplaceholder.typicode.com/users/{id}";
            User user = null;

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse= await response.Content.ReadAsStringAsync();
                    user = JsonSerializer.Deserialize<User>(jsonResponse);
                }
                else
                {
                    ViewBag.Error = "No se pudo obtener la info";
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(user);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
