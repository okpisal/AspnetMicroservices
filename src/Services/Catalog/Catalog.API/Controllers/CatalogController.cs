using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            this.repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(type:typeof(IEnumerable<Product>),StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products=await repository.GetProducts();
            return Ok(products);    
        }
        [HttpGet("{id:length(24)}",Name ="GetProduct")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(type:(typeof(Product)),StatusCodes.Status200OK)]
        public async Task<ActionResult<Product>> GetProduectById(string id)
        {
            var product =await repository.GetProduct(id);
            if (product == null)
            {
                this._logger.LogError($"Proudct with id: {id} not found");
                return NotFound();
            }
            return Ok(product);
        }
        [HttpGet]
        [Route("[action]/category",Name ="GetProductByCategory")]
        [ProducesResponseType(type:typeof(IEnumerable<Product>),StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduectByCategory(string category)
        {
            var products = await repository.GetProductByCategory(category);
            return Ok(products);

        }

        [HttpPost]
        [ProducesResponseType(type:typeof(Product),StatusCodes.Status200OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await repository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(type:typeof(Product),StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProduct([FromBody]Product product)
        {
            return Ok(await repository.UpdateProduct(product));
        }
        [HttpDelete("{id:length(24)}",Name ="DeleteProduct")]
        [ProducesResponseType(type:typeof(Product),StatusCodes.Status200OK)]

        public async Task<IActionResult> DeleteProduct(string id)
        {
            return Ok(await repository.DeleteProduct(id));
        }
    }
}
