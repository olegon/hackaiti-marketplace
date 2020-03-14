using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cart.Service.API.Models;
using Cart.Service.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace cart.service.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly ILogger<CartsController> _logger;
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;

        public CartsController(ILogger<CartsController> logger, IMapper mapper, ICartRepository cartRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _cartRepository = cartRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody]CreateCartRequest payload)
        {
            _logger.LogInformation("CreateCartRequest: {@payload}", payload);

            var cart = await _cartRepository.CreateCart(payload);      

            var response = _mapper.Map<CreateCartResponse>(cart);      

            _logger.LogInformation("CreateCartRequest: {@response}", response);

            return Ok(response);
        }
    }
}
