﻿using System;
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
        private ICurrencyService _currencyCacheService;
        private readonly IMapper _mapper;

        public CurrenciesController(ILogger<CurrenciesController> logger, IMapper mapper, ICurrencyService currencyCacheService)
        {
            _logger = logger;
            _currencyCacheService = currencyCacheService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _currencyCacheService.GetCurrencies();

            return Ok(response);
        }
    }
}
