using LatinoHeat.Data;
using LatinoHeat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatinoHeat.Models
{
	public class Anime
	{
		[Key]
		public int Id { get; set; }

		[NotMapped]
		public IFormFile? Cover { get; set; }

        public string? CoverPath { get; set; }

        [Required]
		public string Title { get; set; }

		[Required]
		public string Description {  get; set; }

		[Required]
		public DateOnly ReleaseDate { get; set; }

		[Required(ErrorMessage = "Please select a Genre.")]
		public string Genre { get; set; }

		[Required]
		public int EpisodeCount { get; set; }

		public string? CreatedBy { get; set; }

		[Required]
		[Range(1, 10)]
		public int Rating { get; set; }


	}
}