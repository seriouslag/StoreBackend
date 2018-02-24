using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurruhBackend.Models;
using SurruhBackend.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SurruhBackend.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {

        private readonly SurruhBackendContext _context;

        public ProductController(SurruhBackendContext context)
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
                            ImageData = po.ProductOption_ImageData
                                .Select(x => x.ImageData)
                                .Select(im => new ImageDataViewModel
                                {
                                    Id = im.Id,
                                    Name = im.Name,
                                    Extension = im.Extension,
                                    CreatedDate = im.CreatedDate,
                                    LastModified = im.LastModified,
                                    ImageId = im.ImageId,
                                    IsVisible = im.IsVisible,
                                    Tags = im.ImageData_Tags
                                        .Select(t => new TagViewModel
                                        {
                                            Id = t.Tag.Id,
                                            Name = t.Tag.Name
                                        }).ToList()
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
                        ImageData = po.ProductOption_ImageData.Select(x => x.ImageData)
                            .Select(im => new ImageDataViewModel
                            {
                                Id = im.Id,
                                Name = im.Name,
                                Extension = im.Extension,
                                CreatedDate = im.CreatedDate,
                                LastModified = im.LastModified,
                                ImageId = im.ImageId,
                                IsVisible = im.IsVisible,
                                Tags = im.ImageData_Tags.Select(t => new TagViewModel
                                {
                                    Id = t.Tag.Id,
                                    Name = t.Tag.Name
                                }).ToList()
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
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
