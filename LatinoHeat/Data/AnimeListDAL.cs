using LatinoHeat.Interface;
using LatinoHeat.Models;

namespace LatinoHeat.Data
{
	public class AnimeListDAL : IDataAccessLayer
	{
		AnimeDbContext _animeDbContext;

		public AnimeListDAL(AnimeDbContext animeDbContext)
		{
			this._animeDbContext = animeDbContext;
		}

		public void CreateAnime(Anime anime)
		{
			_animeDbContext.Animes.Add(anime);
			_animeDbContext.SaveChanges();
		}

		public IEnumerable<Anime> FilterAnime(string AnimeTitle)
		{
			if (AnimeTitle == null)
				AnimeTitle = string.Empty;

			if (AnimeTitle == "")
				return GetAnime();

			IEnumerable<Anime> GetFundraiserName = GetAnime().Where(m => (!string.IsNullOrEmpty(m.Title)
			&& m.Title.ToLower().Contains(AnimeTitle.ToLower()))).ToList();

			if (GetFundraiserName.ToString() == "")
				return GetFundraiserName;

			return GetFundraiserName;
		}

		public IEnumerable<Anime> GetAnime()
		{
			return _animeDbContext.Animes;
		}

		public Anime? GetAnime(int AnimeId)
		{
			Anime? FundraiserFound = _animeDbContext.Animes.Where(p => p.Id == AnimeId).FirstOrDefault();
			return FundraiserFound;
		}

		public void UpdateAnime(Anime anime)
		{
			_animeDbContext.Animes.Update(anime);
			_animeDbContext.SaveChanges();
		}
		public void DeleteAnime(Anime anime)
		{
			_animeDbContext.Animes.Remove(anime);
			_animeDbContext.SaveChanges();
        }	
		public bool AnimeExists(int id)
		{
			return _animeDbContext.Animes.Any(e => e.Id == id);
        }
    }
}
