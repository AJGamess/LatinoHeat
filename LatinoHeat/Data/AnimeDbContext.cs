using LatinoHeat.Models;
using Microsoft.EntityFrameworkCore;

namespace LatinoHeat.Data
{
	public class AnimeDbContext: DbContext
	{
		public AnimeDbContext(DbContextOptions<AnimeDbContext> options)
		: base(options)
		{

		}

		public DbSet<Anime> animes { get; set; }
	}
}
