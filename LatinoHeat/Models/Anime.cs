using System.ComponentModel.DataAnnotations;

namespace LatinoHeat.Models
{
	public class Anime
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public IFormFile Cover { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description {  get; set; }

		[Required]
		public DateOnly ReleaseDate;

		[Required(ErrorMessage = "Please select a Genre.")]
		public string Category { get; set; }

		[Required]
		public int EpisodeCount { get; set; }

		public string? CreatedBy { get; set; }

		[Required]
		[Range(1, 10)]
		public int Rating { get; set; }


	}
}
