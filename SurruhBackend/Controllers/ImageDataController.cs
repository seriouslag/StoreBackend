using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurruhBackend.Models;
using SurruhBackend.ViewModels;

namespace SurruhBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/imageData")]
    public class ImageDataController : Controller
    {
        private readonly SurruhBackendContext _context;

        public ImageDataController(SurruhBackendContext context)
        {
            _context = context;
        }

        // GET: api/imageData
        [HttpGet]
        public IActionResult GetImageData()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageData = _context.ImageData
                .Select(id => new ImageDataViewModel
                {
                    Id = id.Id,
                    ImageId = id.ImageId,
                    IsVisible = id.IsVisible,
                    LastModified = id.LastModified,
                    Name = id.Name,
                    CreatedDate = id.CreatedDate,
                    Extension = id.Extension,
                    Tags = id.ImageData_Tags.Select(x => new TagViewModel
                        {
                            Id = x.Tag.Id,
                            Name = x.Tag.Name
                        }).ToList()
                })
                .Where(id => id.IsVisible == true);

            if(imageData != null)
            {
                return Ok(imageData);
            }

            return NotFound();
        }

        // GET: api/imageData/all
        [HttpGet("all")]
        public IActionResult GetAllImageData()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (User.IsInRole("Admin"))
            {
                return Ok(_context.ImageData
                    .Select(id => new ImageDataViewModel
                    {
                        Id = id.Id,
                        ImageId = id.ImageId,
                        IsVisible = id.IsVisible,
                        LastModified = id.LastModified,
                        Name = id.Name,
                        CreatedDate = id.CreatedDate,
                        Extension = id.Extension,
                        Tags = id.ImageData_Tags.Select(x => new TagViewModel
                        {
                            Id = x.Tag.Id,
                            Name = x.Tag.Name
                        }).ToList()
                    }));
            }
            return GetImageData();
        }

        // GET: api/imageData/5
        [HttpGet("{imageDataId}")]
        public async Task<IActionResult> GetImageData([FromRoute] int imageDataId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ImageDataViewModel imageData = await _context.ImageData.Select(id => new ImageDataViewModel
            {
                Id = id.Id,
                ImageId = id.ImageId,
                LastModified = id.LastModified,
                Name = id.Name,
                CreatedDate = id.CreatedDate,
                Extension = id.Extension,
                IsVisible = id.IsVisible,
                Tags = id.ImageData_Tags.Select(x => new TagViewModel
                {
                    Id = x.Tag.Id,
                    Name = x.Tag.Name
                }).ToList()
            })
            .Where(id => id.IsVisible == true)
            .SingleOrDefaultAsync(id => id.Id == imageDataId);

            if (imageData == null || imageData.ToString() == "[]")
            {
                return NotFound();
            }

            return Ok(imageData);
        }

        // PUT: api/imageDatas/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImageData([FromRoute] int id, [FromBody] ImageData imageData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != imageData.Id)
            {
                return BadRequest();
            }

            _context.Entry(imageData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/imageDatas
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostImageData([FromBody] ImageData imageData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ImageData.Add(imageData);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ImageDataExists(imageData.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetImageData", new { id = imageData.Id }, imageData);
        }

        // DELETE: api/imageDatas/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImageData([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageData = await _context.ImageData.SingleOrDefaultAsync(m => m.Id == id);
            if (imageData == null)
            {
                return NotFound();
            }

            _context.ImageData.Remove(imageData);
            await _context.SaveChangesAsync();

            return Ok(imageData);
        }

        private bool ImageDataExists(int id)
        {
            return _context.ImageData.Any(e => e.Id == id);
        }
    }
}