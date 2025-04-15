using System.Net;
using Catalog.API.Interfaces.Manager;
using Catalog.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreApiResponse;
using MongoDB.Bson;
namespace Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : BaseController
    {
        IProductManager _productManager;
        public CatalogController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ResponseCache(Duration = 30)]
        public IActionResult GetProducts()
        {
           

            try
            {
                var products = _productManager.GetAll();
                return CustomResult("Data loaded successfully", products);
            }
            catch (Exception e)
            {
                return CustomResult(e.Message,HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public IActionResult CreateProduct([FromBody]Product product)
        {
            try
            {
                product.Id = ObjectId.GenerateNewId().ToString();
                bool isSaved = _productManager.Add(product);
                if (isSaved) 
                { 
                    return CustomResult("Product Saved",product,HttpStatusCode.OK);
                }
                return CustomResult("Failed to save ", product, HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return CustomResult(e.Message, HttpStatusCode.BadRequest);
            }
        }
        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult UpdateProduct([FromBody]Product product)
        {
            try
            {
                if(string.IsNullOrEmpty(product.Id))
                {
                    return CustomResult("Product Id is required", HttpStatusCode.NotFound);
                }
                bool isUpdated = _productManager.Update(product.Id,product);
                if (isUpdated)
                {
                    return CustomResult("Product updated", product, HttpStatusCode.OK);
                }
                return CustomResult("Failed to update ", product, HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return CustomResult(e.Message, HttpStatusCode.BadRequest);
            }
        }
        [HttpDelete]
        public IActionResult DeleteProduct(String Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return CustomResult("Product Id is required", HttpStatusCode.NotFound);
                }

                bool isDeleted = _productManager.Delete(Id);

                if (isDeleted)
                {
                    return CustomResult("Product Deleted", HttpStatusCode.OK);
                }
                return CustomResult("Failed To Deleted ", HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return CustomResult(e.Message, HttpStatusCode.BadRequest);
            }
        }
        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult GetById(string Id)
        {
            try
            {
                var products = _productManager.GetById(Id);
                return CustomResult("Data loaded successfully", products);
            }
            catch (Exception e)
            {
                return CustomResult(e.Message, HttpStatusCode.BadRequest);
            }
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public IActionResult GetByCategory(string category)
        {
            try
            {
                var products = _productManager.GetByCategory(category);
                return CustomResult("Data loaded successfully", products);
            }
            catch (Exception e)
            {
                return CustomResult(e.Message, HttpStatusCode.BadRequest);
            }
        }

    }
}
