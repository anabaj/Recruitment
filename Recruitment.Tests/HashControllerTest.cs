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

            var hashController = new HashController(mockFactory.Object);

            var result = await hashController.PostAsync(loginContract);

            var actual = JsonConvert.SerializeObject(((OkObjectResult)result).Value);
            var expected = JsonConvert.SerializeObject(loginHashContract);


            Assert.Equal(expected, actual);

        }
    }
}
