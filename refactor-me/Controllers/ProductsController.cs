﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using refactor_me.Models;
using refactor_me.Repositories;
using refactor_me.Interface;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private ProductsEF db = new ProductsEF();
        private IProductRepository repository = new ProductRepository();

        ////<summary>
        //// 1. GET /products - gets all products.
        ////<param></param>
        ////</summary>
        [ResponseType(typeof(Products))]
        public async Task<IHttpActionResult> GetProducts()
        {
            var result = await repository.GetAllProducts();
            return Ok(result);
        }
        
        ////<summary>
        //// 2. GET /products?name={name} - finds all products matching the specified name.
        ////<param>name</param>
        ////</summary>
        [ResponseType(typeof(Products))]
        [HttpGet]
        public async Task<IHttpActionResult> GetProductByName(string name)
        {
            var result = await repository.GetProduct(name);
            return Ok(result);
        }
        
        ////<summary>
        //// 3. GET /products/{id} - gets the project that matches the specified ID - ID is a GUID.
        ////<param>name</param>
        ////</summary>
        [ResponseType(typeof(Product))]
        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProductById(string id)
        {
            var result = await repository.GetProduct(id);
            return Ok(result.Items.First());
        }

        ////<summary>
        //// 4. POST /products - creates a new product.
        ////<param>name</param>
        ////</summary>
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await repository.AddProduct(product);
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, "Error occurred");
            }

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        ////<summary>
        //// 5. PUT /products/{id} - updates a product.
        ////<param>ProductId</param>
        ////<param>Product</param>
        ////</summary>
        [ResponseType(typeof(string))]
        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutProduct(Guid id, Product item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                return BadRequest();
            }

            bool success = await repository.UpdateProduct(item);
            if (success)
            {
                return Content(HttpStatusCode.OK, "Product updated successfully");
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, "Error Occurred");
            }
        }

        ////<summary>
        // 6. DELETE /products/{id} - deletes a product and its options.
        ////<param>Id</param>
        ////</summary>
        [ResponseType(typeof(string))]
        [Route("{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProduct(Guid id)
        {
            bool result = await repository.DeleteProduct(id);
            if (result)
            {
                return Content(HttpStatusCode.OK, "Product deleted successfully");
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, "Error Occurred");
            }
        }

        ////<summary>
        //// 7. GET /products/{id}/options - finds all options for a specified product.
        ////<param>name</param>
        ////</summary>
        [ResponseType(typeof(ProductOptions))]
        [Route("{productId}/options")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProductOptions(Guid productId)
        {
            var result = await repository.GetAllProductOptions(productId);
            return Ok(result);
        }

        ////<summary>
        //// 8. GET /products/{id}/options/{optionId} - finds the specified product option for the specified product.
        ////<param>ProductId</param>
        ////<param>OptionId</param>
        ////</summary>
        [ResponseType(typeof(ProductOption))]
        [Route("{productId}/options/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProductOption(Guid productId, Guid id)
        {
            var result = await repository.GetProductOption(productId, id);
            return Ok(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        ////<summary>
        //// 9. POST /products/{id}/options - adds a new product option to the specified product.
        ////<param>Option</param>
        ///</summary>
        [ResponseType(typeof(string))]
        [Route("{productId}/options")]
        [HttpPost]
        public async Task<IHttpActionResult> PostProductOption(ProductOption productOption)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await repository.AddProductOption(productOption);
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, "Error occurred");
            }
            
            return Content(HttpStatusCode.OK, "Option added successfully");
        }

        ////<summary>
        //// 10. PUT /products/{id}/options/{optionId} - updates the specified product option.
        ////<param>ProductId</param>
        ////<param>Option</param>
        ////</summary>
        [ResponseType(typeof(string))]
        [Route("{productId}/options/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutProductOption(Guid id, ProductOption productOption)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productOption.Id)
            {
                return BadRequest();
            }

            var result = await repository.UpdateProductOption(productOption);
            if (result)
            {
                return Content(HttpStatusCode.OK, "Option updated successfully");
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, "Error Occurred");
            }
        }

        ////<summary>
        //// 11. DELETE /products/{id}/options/{optionId} - deletes the specified product option.
        ////<param>OptionID</param>
        ////</summary>
        [ResponseType(typeof(string))]
        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProductOption(Guid id)
        {
            bool result = await repository.DeleteProductOption(id);
            if (result)
            {
                return Content(HttpStatusCode.OK, "Option deleted successfully");
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, "Error Occurred");
            }
        }
    }
}