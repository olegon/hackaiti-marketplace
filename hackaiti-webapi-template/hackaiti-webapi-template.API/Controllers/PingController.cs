using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using hackaiti_webapi_template.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hackaiti_webapi_template.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        private readonly ILogger<PingController> _logger;
        private readonly IMapper _mapper;

        public PingController(ILogger<PingController> logger, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PingResponse), 200)]
        public PingResponse Post([FromBody]PingRequest payload)
        {
            _logger.LogInformation("Payload: {@payload}", payload);

            var response = _mapper.Map<PingResponse>(payload);

            _logger.LogInformation("Response: {@response}", response);

            return response;
        }
    }
}
