using System.ComponentModel.DataAnnotations;

public class NewsDto
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string ImageUrl { get; set; }
}
