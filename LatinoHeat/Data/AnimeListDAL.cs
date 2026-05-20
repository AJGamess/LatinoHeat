using LatinoHeat.Interface;
using LatinoHeat.Models;

namespace LatinoHeat.Data
{
	public class AnimeListDAL : IDataAccessLayer
	{
		AnimeDbContext _animeDbContext;

		AnimeListDAL(AnimeDbContext animeDbContext)
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

		public void UpdateUpdate(Anime anime)
		{
			_animeDbContext.Animes.Update(anime);
			_animeDbContext.SaveChanges();
		}
	}
}
