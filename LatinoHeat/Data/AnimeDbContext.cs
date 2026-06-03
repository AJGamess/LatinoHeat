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

		public DbSet<Anime> Animes { get; set; }
		public DbSet<AnimeReaction> AnimeReactions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<AnimeReaction>()
				.HasIndex(r => new { r.AnimeId, r.UserId })
				.IsUnique();
		}
	}
}