using System.Net;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository,
            ILogger<CatalogController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),
            (int)HttpStatusCode.OK)]

        public async Task<ActionResult<IEnumerable<Product>>>
            GetProducts() =>
            Ok(await _repository.
                GetProductsAsync());


        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product),
            (int) HttpStatusCode.OK)]

        public async Task<ActionResult<Product>>
            GetProduct(string id)
        {
            Product product = await _repository
                .GetProductAsync(id);
            if (product == null)
            {
                _logger.LogError($"Product with id: {id}," +
                                 $"not found");
                return NotFound();
            }

            return Ok(product);
        }

        [Route("[action]/{category}",
            Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),
            (int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>>
            GetProductByCategory(string category) =>
            Ok(await _repository
                .GetProductByCategoryAsync(category));

        [HttpPost]
        [ProducesResponseType(typeof(Product),
            (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Product>>
            CreateProduct([FromBody] Product product)
        {
            await _repository.CreateProductAsync(product);
            return CreatedAtRoute("GetProduct",
                new {id = product.Id},
                product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product),
            (int) HttpStatusCode.OK)]
        public async Task<ActionResult>
            UpdateProduct([FromBody] Product product) =>
            Ok(await _repository.UpdateProductAsync(product));

        [HttpDelete("{id:length(24)}",
            Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product),
            (int) HttpStatusCode.OK)]

        public async Task<ActionResult>
            DeleteProduct([FromBody] string id) =>
            Ok(await _repository.DeleteProductAsync(id));
    }
}
