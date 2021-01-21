using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Recruitment.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace Recruitment.Functions
{
    public static class CalculateMD5
    {
        [FunctionName("CalculateMD5")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            //log.LogInformation("Caclulating MD5");

            string loginContractJson = await new StreamReader(req.Body).ReadToEndAsync();
            //LoginContract loginContract = JsonConvert.DeserializeObject<LoginContract>(loginContractJson);

            var hash = GetMD5Hash(loginContractJson);

            LoginHashContract loginHashContract = new LoginHashContract { HashValue = hash };

            return new OkObjectResult(loginHashContract);
        }

        private static string GetMD5Hash(string source)
        {
            var hash = string.Empty;
            using (var md5Hash = MD5.Create())
            {
                var sourceBytes = Encoding.UTF8.GetBytes(source);
                var hashBytes = md5Hash.ComputeHash(sourceBytes);

                hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }
            return hash;

        }
    }


}
