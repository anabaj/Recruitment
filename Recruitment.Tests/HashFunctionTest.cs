using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using Recruitment.Contracts;
using Recruitment.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Recruitment.Tests
{
    public class HashFunctionTest
    {
        [Fact]
        public async void CalculateMD5Test()
        {
            var expected = "6117A05125D5F183CA8BC45D099A418D";
            object result = await CalculateMD5.Run(HttpRequestSetup(null, "hashhashhash"));

            var loginHashContract = (LoginHashContract)((OkObjectResult)result).Value;
            var actual = loginHashContract.HashValue;

            Assert.Equal(expected, actual);

        }


        public HttpRequest HttpRequestSetup(Dictionary<String, StringValues> query, string body)
        {
            var reqMock = new Mock<HttpRequest>();

            reqMock.Setup(req => req.Query).Returns(new QueryCollection(query));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(body);
            writer.Flush();
            stream.Position = 0;
            reqMock.Setup(req => req.Body).Returns(stream);
            return reqMock.Object;
        }
    }
}
