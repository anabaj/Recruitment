using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Recruitment.API.Controllers;
using Recruitment.Contracts;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Recruitment.API.Models;
using Xunit;

namespace Recruitment.Tests
{
    public class HashControllerTest
    {
        [Fact]
        public async void PostTest()
        {
            var loginContract = new LoginContract { Login = "ana", Password = "anaspassword" };
            var loginHashContract = new LoginHashContract { HashValue = "hashhashhashhash" };

            var mockFactory = new Mock<IHttpClientFactory>();
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(loginHashContract), Encoding.UTF8, "application/json"),
            };

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);


            var httpClient = new HttpClient(handlerMock.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);


            Settings app = new Settings() { ApiUrls = new ApiUrls(){ CalculateMD5 = "http://test"} }; 
            var mockOptions = new Mock<IOptions<Settings>>();
            // We need to set the Value of IOptions to be the SampleOptions Class
            mockOptions.Setup(ap => ap.Value).Returns(app);

            var hashController = new HashController(mockFactory.Object, mockOptions.Object);

            var result = await hashController.PostAsync(loginContract);

            var actual = JsonConvert.SerializeObject(((OkObjectResult)result).Value);
            var expected = JsonConvert.SerializeObject(loginHashContract);


            Assert.Equal(expected, actual);

        }
    }
}
