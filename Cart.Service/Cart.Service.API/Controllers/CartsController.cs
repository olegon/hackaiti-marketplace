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

            var response = _mapper.Map<CartResponse>(cart);      

            _logger.LogInformation("CreateCartRequest: {@response}", response);

            return Ok(response);
        }

        [HttpDelete("{cartId}")]
        public async Task<IActionResult> CancelCart([FromRoute]string cartId)
        {
            _logger.LogInformation("CancelCart: {cartId}", cartId);

            var cart = await _cartRepository.CancelCart(cartId);      

            var response = _mapper.Map<CartResponse>(cart);      

            _logger.LogInformation("CancelCart: {@response}", response);

            return Ok(response);
        }

        [HttpPatch("{cartId}/items")]
        public async Task<IActionResult> UpdateCartItem([FromRoute]string cartId, [FromBody]UpdateCartItemRequest payload)
        {
            _logger.LogInformation("UpdateCartItem: {cartId} {@payload}", cartId, payload);

            var cart = await _cartRepository.UpdateCartItem(cartId, payload);      

            var response = _mapper.Map<CartResponse>(cart);      

            _logger.LogInformation("UpdateCartItem: {@response}", response);

            return Ok(response);
        }

        [HttpPost("{cartId}/checkout")]
        public async Task<IActionResult> CartCheckout([FromRoute]string cartId, [FromBody]CartCheckoutRequest payload, [FromHeader(Name = "x-team-control")]string controlId)
        {
            _logger.LogInformation("CartCheckout: {cartId} {@payload} {controlId}", cartId, payload, controlId);

            var cart = await _cartRepository.CartCheckout(cartId, payload, controlId);      

            var response = _mapper.Map<CartResponse>(cart);      

            _logger.LogInformation("CartCheckout: {@response}", response);

            return Ok(response);
        }
    }
}
