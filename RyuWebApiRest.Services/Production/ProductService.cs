using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RyuWebApiRest.DataModel;

namespace RyuWebApiRest.Services.Production
{
    public class ProductService : IProductService
    {
        private AdventureWorks2017Entities _db = new AdventureWorks2017Entities();
        private IMapper _mapper;

        public ProductService()
        {
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Product, DataModel.Product>())
                .CreateMapper();  
        }

        public int AddProduct(Product product)
        {
            try
            {
                var entity = _mapper.Map<DataModel.Product>(product);

                var addedProduct = _db.Products.Add(entity);
                _db.SaveChanges();

                return addedProduct.ProductID;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteProduct(int productId)
        {
            var dbProduct = _db.Products.Find(productId);
            if (dbProduct == null)
                return;

            var trHistories = _db.TransactionHistories.Where(th => th.ProductID == productId);
            var bofToDelete = _db.BillOfMaterials.Where(bof => bof.ComponentID == productId 
                                                            || bof.ProductAssemblyID == productId);
            var piToDelete = _db.ProductInventories.Where(pi => pi.ProductID == productId);
            var pppToDelete = _db.ProductProductPhotoes.Where(ppp => ppp.ProductID == productId);
            var pvToDelete = _db.ProductVendors.Where(pv => pv.ProductID == productId);
            var podToDelete = _db.PurchaseOrderDetails.Where(pod => pod.ProductID == productId);
            var woToDelete = _db.WorkOrders.Where(wo => wo.ProductID == productId);

            _db.WorkOrders.RemoveRange(woToDelete);
            _db.PurchaseOrderDetails.RemoveRange(podToDelete);
            _db.ProductVendors.RemoveRange(pvToDelete);
            _db.ProductProductPhotoes.RemoveRange(pppToDelete);
            _db.ProductInventories.RemoveRange(piToDelete);
            _db.BillOfMaterials.RemoveRange(bofToDelete);
            _db.TransactionHistories.RemoveRange(trHistories);

            _db.Products.Remove(dbProduct);
            _db.SaveChanges();
        }

        public async Task<Product> GetProduct(int prodId)
        {
            var dbProduct = await _db.Products.FindAsync(prodId);
            if (dbProduct == null)
                return null;

            return MapToDto(dbProduct);
        }

        public ICollection<Product> GetProducts()
        {
            return _db.Products.ToArray().Select(p => MapToDto(p)).ToArray();
        }

        public void UpdateProduct(Product product)
        {
            var entity = _mapper.Map<DataModel.Product>(product);

            _db.Entry(entity).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                    throw;
            }
        }

        private Product MapToDto(DataModel.Product product)
        {
            return new Product
            {
                ProductId = product.ProductID,
                Name = product.Name,
                ProductNumber = product.ProductNumber
            };
        }
    }
}
