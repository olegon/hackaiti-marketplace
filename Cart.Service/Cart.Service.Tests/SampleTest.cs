using NUnit.Framework;
using Moq;
using cart.service.API.Controllers;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace cart.service.Tests
{
    public class SampleTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            // var loggerMock = new Mock<ILogger<ProductsController>>();
            // var controller = new ProductsController(loggerMock.Object);
            // var result = controller.Get();
            // Assert.AreEqual(5, result.Count());
            Assert.Pass();
        }
    }
}