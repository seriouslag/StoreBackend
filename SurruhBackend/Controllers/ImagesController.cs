using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurruhBackend.Models;

namespace SurruhBackend.Controllers
{

    [Route("api/images")]
    public class ImagesController : Controller
    {
        private readonly SurruhBackendContext _context;

        public ImagesController(SurruhBackendContext context)
        {
            _context = context;
        }

        // GET: api/Images
        [Produces("application/json")]
        [HttpGet]
        public IActionResult GetImages()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var images = _context.Images
                .Select(i => new ImageViewModel
                {
                    Id = i.Id,
                    Data = i.Data,
                    Height = i.Height,
                    Width = i.Width,
                    ContentType = i.ContentType,
                    Length = i.Length,
                    Name = i.Name,
                    ImageDataId = i.ImageData.Id,
                    IsVisible = i.ImageData.IsVisible
                })
                .Where(i => i.IsVisible == true);

            if (images != null)
            {
                return Ok(images);
            }

            return NotFound();
        }

        [Produces("application/json")]
        [HttpGet("all")]
        public IActionResult GetAllImages()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (User.IsInRole("Admin"))
            {
                var images = Ok(_context.Images);

                if (images != null)
                {
                    return Ok(images);
                }

                return NotFound();

            }

            return GetImages();
        }

        // GET: api/images/5
        [HttpGet("{imageId}")]
        public async Task<IActionResult> GetImageFile([FromRoute] int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ImageViewModel image;
            if (User.IsInRole("Admin"))
            {
                image = await _context.Images
                    .Select(i => new ImageViewModel
                    {
                        Id = i.Id,
                        Data = i.Data,
                        Height = i.Height,
                        Width = i.Width,
                        ContentType = i.ContentType,
                        Length = i.Length,
                        Name = i.Name,
                        ImageDataId = i.ImageData.Id,
                        IsVisible = i.ImageData.IsVisible
                    })
                    .SingleOrDefaultAsync(i => i.Id == imageId);
            } else
            {
                image = await _context.Images
                    .Select(i => new ImageViewModel
                    {
                        Id = i.Id,
                        Data = i.Data,
                        Height = i.Height,
                        Width = i.Width,
                        ContentType = i.ContentType,
                        Length = i.Length,
                        Name = i.Name,
                        ImageDataId = i.ImageData.Id,
                        IsVisible = i.ImageData.IsVisible
                    })
                    .Where(i => i.IsVisible == true)
                    .SingleOrDefaultAsync(i => i.Id == imageId);
            } 

            if (image == null)
            {
               return NotFound();
            }

            return File(new MemoryStream(image.Data), $"{image.ContentType}", $"{image.Name}");
        }

        // PUT: api/Images/5
        [Authorize]
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

        // POST: api/Images
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostImage([FromBody] Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImage", new { id = image.Id }, image);
        }

        // DELETE: api/Images/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var image = await _context.Images.SingleOrDefaultAsync(i => i.Id == id);
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