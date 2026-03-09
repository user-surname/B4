using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using Newtonsoft.Json;
using System.Text;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class CiclosApiService
    {
        private readonly HttpClient _http;
        private readonly string url = "http://localhost:5029/api/v1/Ciclos";
        private string _jwtToken;


        public CiclosApiService()
        {
            _http = new HttpClient();
        }

        public void SetToken(string token)
        {
            _jwtToken = token;
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _jwtToken);
        }

        public async Task<List<CiclosGetDto>> GetAll()
        {
            var response = await _http.GetAsync(url);

            // Comprobar 401 antes de deserializar
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                MessageBox.Show("No estás autorizado. Debes iniciar sesión.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null; // o new List<CiclosGetDto>()
            }

            var json = await response.Content.ReadAsStringAsync();

            // Deserializar correctamente
            var result = JsonConvert.DeserializeObject<ApiResponse<List<CiclosGetDto>>>(json);

            return result.data; // devuelve la lista real
        }

        public async Task<CiclosGetDto> GetById(int id)
        {
            var response = await _http.GetAsync($"{url}/{id}");

            // Comprobar 401 antes de deserializar
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                MessageBox.Show("No estás autorizado. Debes iniciar sesión.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null; // o new List<CiclosGetDto>()
            }

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ApiResponse<CiclosGetDto>>(json);

            return result.data;
        }

        public async Task Insert(CiclosPostDto ciclo)
        {
            var json = JsonConvert.SerializeObject(ciclo);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _http.PostAsync(url, content);
        }

        public async Task Delete(int id)
        {
            await _http.DeleteAsync($"{url}/{id}");
        }
    }
}