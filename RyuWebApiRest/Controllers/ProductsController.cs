using System;
using System.Collections.Generic;
using System.Net;
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
            Serilog.Log.Information("Products GET products - STARTED");

            try
            {
                return _prodSrv.GetProducts();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Products GET products - EXCEPTION");
                System.Diagnostics.Trace.WriteLine($"Products Exception {ex.Message}");
                throw;
            }
        }

        // GET api/products/5
        public async Task<IHttpActionResult> Get(int id)
        {
            Serilog.Log.Information("Products GET product - STARTED");

            try
            {
                return Ok(await _prodSrv.GetProduct(id));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, $"Products GET product - EXCEPTION, {id}");
                System.Diagnostics.Trace.WriteLine($"Products Exception {ex.Message}");
                throw;
            }
        }

        // POST api/products
        public IHttpActionResult Post([FromBody]Product product)
        {
            Serilog.Log.Information("Products POST products - STARTED");

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var productId = _prodSrv.AddProduct(product);

                return CreatedAtRoute("DefaultApi", new { id = productId }, product);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Products POST product - EXCEPTION, {@product}", product);
                System.Diagnostics.Trace.WriteLine($"Products Exception {ex.Message}");
                throw;
            }
        }

        // PUT api/products/5
        public IHttpActionResult Put(int id, [FromBody]Product product)
        {
            Serilog.Log.Information("Products PUT products - STARTED");

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != product.ProductId)
                    return BadRequest();

                _prodSrv.UpdateProduct(product);

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Products PUT product - EXCEPTION, {id} {@product}", id, product);
                System.Diagnostics.Trace.WriteLine($"Products Exception {ex.Message}");
                throw;
            }
        }

        // DELETE api/products/5
        public void Delete(int id)
        {
            Serilog.Log.Information("Products DELETE products - STARTED");

            try
            {
                _prodSrv.DeleteProduct(id);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Products DELETE product - EXCEPTION, {id}", id);
                System.Diagnostics.Trace.WriteLine($"Products Exception {ex.Message}");
                throw;
            }
        }
    }
}
