using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Recruitment.API.Models;
using Recruitment.Contracts;

namespace Recruitment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HashController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<Settings> _appSettings;

        public HashController(IHttpClientFactory httpClientFactory, IOptions<Settings> appSettings )
        {
            _httpClient = httpClientFactory.CreateClient();
            _appSettings = appSettings;
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] LoginContract loginContract)
        {
            var uri = _appSettings.Value.ApiUrls.CalculateMD5;
            var response = await _httpClient.PostAsync(uri, new StringContent(JsonConvert.SerializeObject(loginContract), Encoding.UTF8, "application/json"));

            var responseData = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<LoginHashContract>(responseData);

            return Ok(result);
        }
    }
}
