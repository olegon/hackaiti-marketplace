using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Product.Service.API.Exceptions;
using Product.Service.API.Model;
using Product.Service.API.Repositories;

namespace product.service.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductsController(ILogger<ProductsController> logger, IMapper mapper, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productRepository.GetAllProducts();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById([FromRoute]string id)
        {
             _logger.LogInformation("GetProductById: {id}", id);

            var product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<ProductResponse>(product);

            _logger.LogInformation("GetProductById - ProductResponse: {@response}", response);

            return Ok(response);
        }

        [HttpGet("sku/{sku}")]
        public async Task<IActionResult> GetProductBySKU([FromRoute]string sku)
        {
            _logger.LogInformation("GetProductBySKU: {sku}", sku);

            var product = await _productRepository.GetProductBySku(sku);

            if (product == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<ProductResponse>(product);

            _logger.LogInformation("GetProductBySKU - ProductResponse: {@response}", response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody]CreateProductRequest payload)
        {
            _logger.LogInformation("AddProduct: {@payload}", payload);

            var product = _mapper.Map<Product.Service.API.Entities.Product>(payload);

            try
            {
                await _productRepository.AddProduct(product);
            }
            catch (DuplicatedSkuException ex)
            {
                return UnprocessableEntity(ex.Message);
            }

            var response = _mapper.Map<ProductResponse>(product);

            _logger.LogInformation("AddProduct - ProductResponse: {@response}", response);

            return Created($"/products/{product.Id}", product);
        }
    }
}
