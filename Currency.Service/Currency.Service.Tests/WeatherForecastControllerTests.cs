using NUnit.Framework;
using Moq;
using currency.service.API.Controllers;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace currency.service.Tests
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
            // var loggerMock = new Mock<ILogger<CurrenciesController>>();
            // var controller = new CurrenciesController(loggerMock.Object);
            // var result = controller.Get();
            // Assert.AreEqual(5, result.Count());
            Assert.Pass();
        }
    }
}