using NUnit.Framework;
using Moq;
using hackaiti_webapi_template.API.Controllers;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace hackaiti_webapi_template.Tests
{
    public class WeatherForecastControllerTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);
            var result = controller.Get();
            Assert.AreEqual(5, result.Count());
        }
    }
}