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

        [HttpPost("Create")]
        public async Task<ActionResult<Project>> Create(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return Ok(project);
        }
        
        [HttpDelete("DeleteProject/{id}")]
        public async Task<ActionResult<Project>> DeleteProject(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Images) //include all project sections
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return NotFound("Project not found");

            // delete images from server
            foreach (var img in project.Images)
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.ImageUrl);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // delete images from database
            _context.ProjectImages.RemoveRange(project.Images);
            
            //delete the project
            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();

            return Ok("Deleted successfully");
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

       //photos EndPoints
        [HttpPost("UploadImage/{projectId}")]
        public async Task<IActionResult> UploadImage(int projectId, IFormFile file)
        {
        // 1. Validate file
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        // 2. Create images folder if it doesn't exist
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // 3. Generate unique file name
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        // 4. Full physical path
        var fullPath = Path.Combine(folderPath, fileName);

        // 5. Save file to server
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // 6. Save to database
        var image = new ProjectImage
        {
            ImageUrl = $"images/{fileName}", // matches folder
            ProjectId = projectId
        };

        _context.ProjectImages.Add(image);
        await _context.SaveChangesAsync();

        // 7. Return result
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