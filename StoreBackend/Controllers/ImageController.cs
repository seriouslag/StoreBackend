using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreBackend.Models;

namespace StoreBackend.Controllers
{
    [Route("api/image"), Route("api/images")]
    public class ImageController : Controller
    {
        private readonly Context _context;

        public ImageController(Context context)
        {
            _context = context;
        }

        // GET: api/image
        [Produces("application/json")]
        [HttpGet]
        public IActionResult GetImage()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageData = _context.Images
                .Select(id => new ImageViewModel
                {
                    Id = id.Id,
                    IsActivated = id.IsActivated,
                    LastModified = id.LastModified,
                    Name = id.Name,
                    CreatedDate = id.CreatedDate,
                    // Content = id.Content,
                    ContentType = id.ContentType,
                    Height = id.Height,
                    Width = id.Width
                })
                .Where(id => id.IsActivated == true);

            if(imageData != null)
            {
                return Ok(imageData);
            }

            return NotFound();
        }

        // GET: api/image/all
        [Produces("application/json")]
        [HttpGet("all")]
        public IActionResult GetAllImage()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (User.IsInRole("Admin"))
            {
                return Ok(_context.Images
                    .Select(id => new ImageViewModel
                    {
                        Id = id.Id,
                        IsActivated = id.IsActivated,
                        LastModified = id.LastModified,
                        Name = id.Name,
                        CreatedDate = id.CreatedDate,
                        // Content = id.Content,
                        ContentType = id.ContentType,
                        Height = id.Height,
                        Width = id.Width
                    }));
            }
            return GetImage();
        }

        // GET: api/image/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetImage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ImageViewModel image = await _context.Images
                .Select(i => new ImageViewModel
                {
                    Id = i.Id,
                    LastModified = i.LastModified,
                    Name = i.Name,
                    CreatedDate = i.CreatedDate,
                    IsActivated = i.IsActivated,
                    // Content = id.Content,
                    ContentType = i.ContentType,
                    Height = i.Height,
                    Width = i.Width
                })
                .Where(i => i.IsActivated == true)
                .SingleOrDefaultAsync(i => i.Id == id);

            if (image == null || image.ToString() == "[]")
            {
                return NotFound();
            }

            return Ok(image);
        }

        // GET: api/image/file/5
        [HttpGet("file/{id}")]
        public async Task<IActionResult> GetImageFile([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var image = await _context.Images
                .SingleOrDefaultAsync(i => i.Id == id);

            if (image == null || image.ToString() == "[]")
            {
                return NotFound();
            }
            
            return File(image.Content, image.ContentType, image.FileName());
        }

        // PUT: api/image/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage([FromRoute] int id, [FromBody] Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != image.Id)
            {
                return BadRequest();
            }

            _context.Entry(image).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
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

        // POST: api/image
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostImage([FromBody] Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Images.Add(image);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ImageExists(image.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetImageData", new { id = image.Id }, image);
        }

        // DELETE: api/image/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var image = await _context.Images.SingleOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return Ok(image);
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(i => i.Id == id);
        }
    }
}