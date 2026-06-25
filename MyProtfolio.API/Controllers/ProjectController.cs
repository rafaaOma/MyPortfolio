using Microsoft.AspNetCore.Mvc;
using MyProtfolio.API.Data;
using Microsoft.EntityFrameworkCore;

namespace ProjectController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
     public class ProjectController : ControllerBase
    {
        private readonly PortfolioDbContext _context;
        public  ProjectController(PortfolioDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Project>>> GetAll()
        {
            var projects = await _context.Projects
                .Include(p => p.Skills)
                .Include(p => p.Images)
                .ToListAsync();

            return Ok(projects);
        }

        [HttpPost("UploadImage/{projectId}")]
        public async Task<IActionResult> UploadImage(int projectId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest();

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);//uniqe name for image
            var path = Path.Combine("wwwroot/images", fileName);

            using (var stream = new FileStream(path, FileMode.Create))//move file to server
            {
                await file.CopyToAsync(stream);
            }

            var image = new ProjectImage
            {
                ImageUrl = $"images/{fileName}",
                ProjectId = projectId
            };

            _context.ProjectImages.Add(image);
            await _context.SaveChangesAsync();

            return Ok(image);
        }
        [HttpDelete("DeleteImage/{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.ProjectImages.FindAsync(id);

            if (image == null)
                return NotFound();

            var path = Path.Combine("wwwroot/images", image.ImageUrl);
            
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _context.ProjectImages.Remove(image);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}