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
                    IsVisible = id.IsVisible,
                    LastModified = id.LastModified,
                    Name = id.Name,
                    CreatedDate = id.CreatedDate,
                    // Content = id.Content,
                    ContentType = id.ContentType,
                    Height = id.Height,
                    Width = id.Width
                })
                .Where(id => id.IsVisible == true);

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
                        IsVisible = id.IsVisible,
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
                    IsVisible = i.IsVisible,
                    // Content = id.Content,
                    ContentType = i.ContentType,
                    Height = i.Height,
                    Width = i.Width
                })
                .Where(i => i.IsVisible == true)
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
        public async Task<IActionResult> PutImageData([FromRoute] int id, [FromBody] Image image)
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

        // POST: api/image
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostImageData([FromBody] Image image)
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
                if (ImageDataExists(image.Id))
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
        public async Task<IActionResult> DeleteImageData([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageData = await _context.Images.SingleOrDefaultAsync(m => m.Id == id);
            if (imageData == null)
            {
                return NotFound();
            }

            _context.Images.Remove(imageData);
            await _context.SaveChangesAsync();

            return Ok(imageData);
        }

        private bool ImageDataExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}