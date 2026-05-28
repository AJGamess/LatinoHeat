using LatinoHeat.Models;

namespace LatinoHeat.Interface
{
	public interface IDataAccessLayer
	{
		IEnumerable<Anime> GetAnime();
		void CreateAnime(Anime Anime);
		Anime? GetAnime(int AnimeId);
		void UpdateAnime(Anime anime);
		void DeleteAnime(Anime anime);
		bool AnimeExists(int id);
		IEnumerable<Anime> FilterAnime(string AnimeTitle);
	}
}
