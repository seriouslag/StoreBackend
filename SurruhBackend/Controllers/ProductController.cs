using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurruhBackend.Models;
using SurruhBackend.ViewModels;

namespace SurruhBackend.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {

        private readonly Context _context;

        public ProductController(Context context)
        {
            _context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
            var products = _context.Products
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductDescription = p.ProductDescription,
                    ProductOptions = p.ProductOptions
                        .Select(po => new ProductOptionViewModel
                        {
                            Id = po.Id,
                            Name = po.Name,
                            Price = po.Price,
                            ProductId = po.ProductId,
                            Images = po.Images
                                .Select(x => x.Image)
                                .Select(i => new ImageViewModel
                                {
                                    Id = i.Id,
                                    Name = i.Name,
                                    CreatedDate = i.CreatedDate,
                                    LastModified = i.LastModified,
                                    IsVisible = i.IsVisible,
                                    // Content = i.Content,
                                    ContentType = i.ContentType,
                                    Height = i.Height,
                                    Width = i.Width
                                }).ToList()
                        }).ToList(),
                });

            if (products != null && products.ToString() != "[]")
            {
                return Ok(products);
            }

            return NotFound();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _context.Products
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductDescription = p.ProductDescription,
                    ProductOptions = p.ProductOptions.Select(po => new ProductOptionViewModel
                    {
                        Id = po.Id,
                        Name = po.Name,
                        Price = po.Price,
                        ProductId = po.ProductId,
                        Images = po.Images
                            .Select(x => x.Image)
                            .Select(i => new ImageViewModel
                            {
                                Id = i.Id,
                                Name = i.Name,
                                CreatedDate = i.CreatedDate,
                                LastModified = i.LastModified,
                                IsVisible = i.IsVisible,
                                // Content = i.Content,
                                ContentType = i.ContentType,
                                Height = i.Height,
                                Width = i.Width
                            }).ToList()
                    }).ToList(),
                })
                .SingleOrDefaultAsync(p => p.Id == id);

            if (product != null && product.ToString() != "[]")
            {
                return Ok(product);
            }


           return NotFound();
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public void Delete(int id)
        {
        }
    }
}
