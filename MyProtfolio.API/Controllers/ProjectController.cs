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
        // Validate file
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        //Create images folder if it doesn't exist
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        //generate unique file name
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        // Full physical path
        var fullPath = Path.Combine(folderPath, fileName);

        //Save file to server
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        //Save to database
        var image = new ProjectImage
        {
            ImageUrl = $"images/{fileName}", // matches folder
            ProjectId = projectId
        };

        _context.ProjectImages.Add(image);
        await _context.SaveChangesAsync();

        //Return result
        return Ok(image);
        }
        [HttpDelete("DeleteImage/{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.ProjectImages.FindAsync(id);

            if (image == null)
                return NotFound();

            var path = Path.Combine("wwwroot", image.ImageUrl);
            
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _context.ProjectImages.Remove(image);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //Name EndPoints for edit name update and delete
        [HttpPut("UpdateName/{id}")]
        public async Task<IActionResult> UpdateName(int id, [FromBody] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name cannot be empty");

            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            project.ProjectName = name;

            await _context.SaveChangesAsync();

            return Ok(project);
        }

        //Description EndPoints for edit project Description update and delete
        [HttpPut("UpdateDescription/{id}")]
        public async Task<IActionResult> UpdateDescription(int id, [FromBody] string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return BadRequest("description cannot be empty");

            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            project.Description = description;

            await _context.SaveChangesAsync();

            return Ok(project);
        }

        //for skills adding new skill, update, delete.
        [HttpPost("{projectId}/skills")]
        public async Task<IActionResult> AddProjectSkill(int projectId,[FromBody] string skillName)
        {
            if (string.IsNullOrWhiteSpace(skillName))
                return BadRequest("Skill name cannot be empty");

            var project = await _context.Projects.FindAsync(projectId);

            if (project == null)
                return NotFound("Project not found");

            var skill = new ProjectSkill
            {
                Name = skillName,
                ProjectId = projectId
            };

            _context.ProjectSkills.Add(skill);
            await _context.SaveChangesAsync();

            return Ok(skill);
        }


        [HttpPut("skills/{id}")]
        public async Task<IActionResult> UpdateProjectSkill(int id, [FromBody] string skillName)
        {
            if (string.IsNullOrWhiteSpace(skillName))
                return BadRequest("Skill name cannot be empty");

            var skill = await _context.ProjectSkills.FindAsync(id);

            if (skill == null)
                return NotFound("Skill not found");

            skill.Name = skillName;

            await _context.SaveChangesAsync();

            return Ok(skill);
        }


        [HttpDelete("skills/{id}")]
        public async Task<IActionResult> DeleteProjectSkill(int id)
        {
            var skill = await _context.ProjectSkills.FindAsync(id);

            if (skill == null)
                return NotFound("Skill not found");

            _context.ProjectSkills.Remove(skill);
            await _context.SaveChangesAsync();

            return Ok("Deleted successfully");
        }

        [HttpGet("{projectId}/skills")]
        public async Task<IActionResult> GetProjectSkills(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.Skills)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return NotFound("Project not found");

            return Ok(project.Skills);
        }
    }
}