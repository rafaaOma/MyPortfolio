using System.Text.Json.Serialization;
public class ProjectImage
{
    public int Id { get; set; }

    public string ImageUrl { get; set; } // path أو URL

    public int ProjectId { get; set; }

    [JsonIgnore] 
    public Project Project { get; set; }
}