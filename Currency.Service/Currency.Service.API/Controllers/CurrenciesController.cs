using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Currency.Service.API.Model;
using Currency.Service.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Currency.Service.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrenciesController : ControllerBase
    {
        private readonly ILogger<CurrenciesController> _logger;
        private ICurrencyService _currencyCacheService;
        private readonly IMapper _mapper;

        public CurrenciesController(ILogger<CurrenciesController> logger, IMapper mapper, ICurrencyService currencyCacheService)
        {
            _logger = logger;
            _currencyCacheService = currencyCacheService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetCurrenciesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var response = await _currencyCacheService.GetCurrencies();

            return Ok(response);
        }
    }
}
