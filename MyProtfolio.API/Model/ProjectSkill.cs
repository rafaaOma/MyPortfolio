using System.Text.Json.Serialization;
public class ProjectSkill
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int ProjectId { get; set; }
    [JsonIgnore]
    public Project Project { get; set; }
}