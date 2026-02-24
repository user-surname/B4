using B4.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace B4.Web.Controllers
{
    public class DemoController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBase;

        public DemoController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _apiBase = config.GetValue<string>("Api:BaseUrl")
                ?? throw new InvalidOperationException("Api:BaseUrl no está configurado.");
        }

        [HttpGet]
        public IActionResult Upload() => View(new DemoUploadViewModel());

        // -------------------------
        // POST: Login + Subir (POST)
        // -------------------------
        [HttpPost]
        public async Task<IActionResult> Upload(DemoUploadViewModel model)
        {
            var client = _httpClientFactory.CreateClient();

            // 1) LOGIN -> token
            var token = await GetTokenAsync(client, model.Email, model.Password);
            if (string.IsNullOrWhiteSpace(token))
            {
                ViewBag.StatusCode = 401;
                ViewBag.Result = "LOGIN FAILED: token vacío o respuesta inesperada";
                return View("Result");
            }

            // 2) POST DataActuals con Bearer
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var postUrl = $"{_apiBase}/api/v1/DataActuals/{model.Planta}/{model.Ejercicio}/{model.Mes}/{model.Tipo}";
            var postRes = await client.PostAsync(
                postUrl,
                new StringContent(model.JsonBody, Encoding.UTF8, "application/json")
            );

            var postText = await postRes.Content.ReadAsStringAsync();

            ViewBag.StatusCode = postRes.StatusCode;
            ViewBag.Result = TryPrettyJson(postText);

            return View("Result");
        }

        // -------------------------
        // POST: Login + Consultar (GET)
        // -------------------------
        [HttpPost]
        public async Task<IActionResult> GetActuals(DemoUploadViewModel model)
        {
            var client = _httpClientFactory.CreateClient();

            // 1) LOGIN -> token
            var token = await GetTokenAsync(client, model.Email, model.Password);
            if (string.IsNullOrWhiteSpace(token))
            {
                ViewBag.StatusCode = 401;
                ViewBag.Result = "LOGIN FAILED: token vacío o respuesta inesperada";
                return View("Result");
            }

            // 2) GET DataActuals con Bearer
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // OJO: tu API actual pide epígrafe en ruta.
            // Para demo usamos el 100 (o el que exista).
            var epigrafeDemo = 100;
            var getUrl = $"{_apiBase}/api/v1/DataActuals/{model.Planta}/{model.Ejercicio}/{epigrafeDemo}";

            var getRes = await client.GetAsync(getUrl);
            var getText = await getRes.Content.ReadAsStringAsync();

            ViewBag.StatusCode = getRes.StatusCode;
            ViewBag.Result = TryPrettyJson(getText);

            return View("Result");
        }

        // -------------------------
        // Helpers
        // -------------------------
        private async Task<string?> GetTokenAsync(HttpClient client, string email, string password)
        {
            var loginUrl = $"{_apiBase}/api/v1/Auth/login";
            var loginBody = JsonSerializer.Serialize(new { email, password });

            var loginRes = await client.PostAsync(
                loginUrl,
                new StringContent(loginBody, Encoding.UTF8, "application/json")
            );

            var loginText = await loginRes.Content.ReadAsStringAsync();

            if (!loginRes.IsSuccessStatusCode)
                return null;

            // Esperado: { ..., "data": { "token": "..." } }
            try
            {
                var root = JsonDocument.Parse(loginText).RootElement;

                // Si data es objeto:
                if (root.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Object)
                {
                    if (data.TryGetProperty("token", out var tok))
                        return tok.GetString();
                }

                // Si data fuese array:
                if (root.TryGetProperty("data", out data) && data.ValueKind == JsonValueKind.Array && data.GetArrayLength() > 0)
                {
                    var first = data[0];
                    if (first.ValueKind == JsonValueKind.Object && first.TryGetProperty("token", out var tok))
                        return tok.GetString();
                }
            }
            catch
            {
                // ignore
            }

            return null;
        }

        private static string TryPrettyJson(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var trimmed = text.Trim();
            var looksJson =
                (trimmed.StartsWith("{") && trimmed.EndsWith("}")) ||
                (trimmed.StartsWith("[") && trimmed.EndsWith("]"));

            if (!looksJson)
                return text;

            try
            {
                using var doc = JsonDocument.Parse(trimmed);
                return JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
            }
            catch
            {
                return text;
            }
        }
    }
}