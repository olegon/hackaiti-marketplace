using NUnit.Framework;
using Moq;
using hackaiti_webapi_template.API.Controllers;
using Microsoft.Extensions.Logging;
using System.Linq;
using hackaiti_webapi_template.API.Models;
using AutoMapper;
using hackaiti_webapi_template.API.Infrastructure.AutoMapper;

namespace hackaiti_webapi_template.Tests
{
    public class PingControllerTests
    {
        private IMapper _mapper;
        private Mock<ILogger<PingController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PingProfile>();
            }).CreateMapper();

            _loggerMock = new Mock<ILogger<PingController>>();
        }

        [Test]
        public void Test1()
        {
            var controller = new PingController(_loggerMock.Object, _mapper);
            var payload = new PingRequest()
            {
                SomeId = "123"
            };

            var response = controller.Post(payload);

            Assert.AreEqual(payload.SomeId, response.SomeId);
        }
    }
}