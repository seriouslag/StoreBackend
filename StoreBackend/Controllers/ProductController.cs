using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreBackend.Models;
using StoreBackend.ViewModels;

namespace StoreBackend.Controllers
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
        public IActionResult GetProducts()
        {
            System.Console.WriteLine("*************************" + _context.Products.Single(p => p.Id == 1).IsActivated);
            var products = _context.Products
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductDescription = p.ProductDescription,
                    CreatedDate = p.CreatedDate,
                    LastModified = p.LastModified,
                    IsActivated = p.IsActivated,
                    ProductOptions = p.ProductOptions
                        .Select(po => new ProductOptionViewModel
                        {
                            Id = po.Id,
                            Name = po.Name,
                            Price = po.Price,
                            ProductId = po.ProductId,
                            IsActivated = po.IsActivated,
                            CreatedDate = po.CreatedDate,
                            LastModified = po.LastModified,
                            ProductOptionDescription = po.ProductOptionDescription,
                            Images = po.Images
                                .Select(x => x.Image)
                                .Select(i => new ImageViewModel
                                {
                                    Id = i.Id,
                                    Name = i.Name,
                                    CreatedDate = i.CreatedDate,
                                    LastModified = i.LastModified,
                                    IsActivated = i.IsActivated,
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
        public async Task<IActionResult> GetProduct(int id)
        {

            
            var product = await _context.Products
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsActivated = p.IsActivated,
                    LastModified = p.LastModified, 
                    CreatedDate = p.CreatedDate,
                    ProductDescription = p.ProductDescription,
                    ProductOptions = p.ProductOptions.Select(po => new ProductOptionViewModel
                    {
                        Id = po.Id,
                        Name = po.Name,
                        Price = po.Price,
                        ProductId = po.ProductId,
                        LastModified = po.LastModified,
                        IsActivated = po.IsActivated,
                        CreatedDate = po.CreatedDate,
                        ProductOptionDescription = po.ProductOptionDescription,
                        Images = po.Images
                            .Select(x => x.Image)
                            .Select(i => new ImageViewModel
                            {
                                Id = i.Id,
                                Name = i.Name,
                                CreatedDate = i.CreatedDate,
                                LastModified = i.LastModified,
                                IsActivated = i.IsActivated,
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
        public async Task<IActionResult> Post([FromBody]Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Products.Add(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductExistsByName(product.Name))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProducts", new { id = product.Id }, product);
        }

        // PUT api/<controller>/5

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody]Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;
        

            try
            {
                await _context.SaveChangesAsync();
                System.Console.WriteLine("updating");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExistsById(id))
                {
                    return NotFound();
                }
                else
                {
                    Console.WriteLine("Failed");
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        private bool ProductExistsById(int id)
        {
            return _context.Products.Any(p => p.Id == id);
        }

        private bool ProductExistsByName(string name)
        {
            return _context.Products.Any(p => p.Name == name);
        }
    }
}
