using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recruitment.Contracts;

namespace Recruitment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HashController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public HashController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] LoginContract loginContract)
        {
            var uri = "http://localhost:7071/api/CalculateMD5";
            var response = await _httpClient.PostAsync(uri, new StringContent(JsonConvert.SerializeObject(loginContract), Encoding.UTF8, "application/json"));

            var responseData = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<LoginHashContract>(responseData);

            return Ok(result);
        }
    }
}
