using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica2.Models;

namespace PruebaTecnica2.Controllers
{
    public class CommentsController : Controller
    {
        private readonly HttpClient _httpClient;
        public CommentsController(IHttpClientFactory httpClientFactory) { 
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public IActionResult AddComment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            string apiUrl = $"https://jsonplaceholder.typicode.com/posts/{comment.postId}/comments";
            var jsonContent = new StringContent(JsonSerializer.Serialize(comment), Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(apiUrl, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Comentario agregado correctamente";
                    return View("AddComment", comment);
                }
                else
                {
                    ViewBag.Message = "No se pudo agregar el comentario";
                }
            }
            catch (Exception ex) 
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }
       
    }
}
