using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Currency.Service.API.Model;
using Currency.Service.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace currency.service.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrenciesController : ControllerBase
    {
        private readonly ILogger<CurrenciesController> _logger;
        private ICurrencyCacheService _currencyCacheService;
        private readonly IMapper _mapper;

        public CurrenciesController(ILogger<CurrenciesController> logger, IMapper mapper, ICurrencyCacheService currencyCacheService)
        {
            _logger = logger;
            _currencyCacheService = currencyCacheService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currencies = await _currencyCacheService.GetCurrencies();

            var response = new GetCurrenciesResponse()
            {
                Currencies = _mapper.Map<IEnumerable<GetCurrenciesResponse.Currency>>(currencies)
            };

            return Ok(response);
        }
    }
}
