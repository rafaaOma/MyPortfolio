using Microsoft.AspNetCore.Mvc;
using MyProtfolio.API.Data;
namespace SkillsController.Controllers
{
    [ApiController]
    [Route("api/SkillsModification")]
    public class SkillsModificationController : ControllerBase
    {
        private readonly PortfolioDbContext _context;
        public SkillsModificationController(PortfolioDbContext context)
        {
            _context = context;
        }
        

   [HttpGet("GetAll")]
    public IActionResult GetAll([FromQuery] string category)//fetching data from data base for specific catagories
    {
        var skills = _context.Skills
        .Where(s => s.Category == category)
        .ToList();

        return Ok(skills);
    }

    [HttpPost("AddSkill")]
    public IActionResult AddSkill(Skill skill)//add new skill
    {
        _context.Skills.Add(skill);
        _context.SaveChanges();
        return Ok(skill);
    }
     [HttpDelete("Delete/{id}")]
    public IActionResult Delete(int id)
    {
        var skill = _context.Skills.Find(id);
        if (skill == null) return NotFound();

        _context.Skills.Remove(skill);
        _context.SaveChanges();


        return Ok();
    }
    [HttpPut("UpdateSkill")]
    public IActionResult UpdateSkill(Skill skill)
    {
    var existing = _context.Skills.Find(skill.Id);

    if (existing == null)
        return NotFound();

    existing.Name = skill.Name;
    existing.Category = skill.Category;

    _context.SaveChanges();

    return Ok(existing);
    }
    }
    
}