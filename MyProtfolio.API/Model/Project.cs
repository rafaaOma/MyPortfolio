public class Project
{
    public int Id { get; set; }
    public string ProjectName { get; set; }
    public string Description { get; set; }
    public List<ProjectSkill> Skills { get; set; } = new();
    public List<ProjectImage> Images { get; set; } = new();
}