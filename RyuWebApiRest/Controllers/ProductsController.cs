using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RyuWebApiRest.Services.Production;
using System.Threading.Tasks;


namespace RyuWebApiRest.Controllers
{
    public class ProductsController : ApiController
    {
        IProductService _prodSrv = new ProductService();

        // GET api/products
        public IEnumerable<Product> Get()
        {
            return _prodSrv.GetProducts();
        }

        // GET api/products/5
        public async Task<IHttpActionResult> Get(int id)
        {
            return Ok(await _prodSrv.GetProduct(id));
        }

        // POST api/products
        public IHttpActionResult Post([FromBody]Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productId = _prodSrv.AddProduct(product);

            return CreatedAtRoute("DefaultApi", new { id = productId }, product);
        }

        // PUT api/products/5
        public IHttpActionResult Put(int id, [FromBody]Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != product.ProductId)
                return BadRequest();

            _prodSrv.UpdateProduct(product);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE api/products/5
        public void Delete(int id)
        {
            _prodSrv.DeleteProduct(id);
        }
    }
}
