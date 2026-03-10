using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Services
{
    public class AuthService
    {
        private readonly HttpClient _http = new HttpClient();
        private readonly string _url = "http://localhost:5029/api/v1/auth/login";

        public async Task<string> Login(string email, string password)
        {
            var payload = new { email, password };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(_url, content);
            // Comprobar 401 antes de deserializar
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                MessageBox.Show("Email o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null; // o new List<CiclosGetDto>()
            }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            // Deserializamos como objeto dinámico para acceder a data.token
            dynamic result = JsonConvert.DeserializeObject(json);

            // Retornamos el token dentro de data
            return (string)result.data.token;
        }
    }
}