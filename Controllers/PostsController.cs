using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using PruebaTecnica2.Models;

namespace PruebaTecnica2.Controllers
{
    public class PostsController : Controller
    {
        private readonly HttpClient _httpClient;

        public PostsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? userId, bool? loadData)
        {
            List<Post> posts = new List<Post>();
            if (loadData.HasValue && loadData.Value)
            {
                string apiUrl = "https://jsonplaceholder.typicode.com/posts";

                try
                {
                    var response = await _httpClient.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        posts = JsonSerializer.Deserialize<List<Post>>(jsonResponse);
                        if (userId.HasValue)
                        {
                            posts = posts.Where(post => post.userId == userId.Value).ToList();
                        }

                    }
                    else
                    {
                        ViewBag.Error = "No se pudo obtener la información.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Ocurrió un error: {ex.Message}";
                }
            }

            return View(posts);
        }
        // Acción para ver los comentarios de un post
        [HttpGet]
        public async Task<IActionResult> ViewComments(int postId)
        {
            List<Comment> comments = new List<Comment>();
            string apiUrl = $"https://jsonplaceholder.typicode.com/posts/{postId}/comments";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    comments = JsonSerializer.Deserialize<List<Comment>>(jsonResponse);

                    ViewData["postId"] = postId;
                }
                else
                {
                    ViewBag.Error = "No se pudo obtener los comentarios.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ocurrió un error: {ex.Message}";
            }

            // Regresar la vista con los comentarios
            return View(comments);
        }

        // Acción para crear un nuevo comentario
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
