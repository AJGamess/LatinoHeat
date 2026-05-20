using LatinoHeat.Models;

namespace LatinoHeat.Interface
{
	public interface IDataAccessLayer
	{
		IEnumerable<Anime> GetAnime();
		void CreateAnime(Anime Anime);
		Anime? GetAnime(int AnimeId);
		void UpdateUpdate(Anime anime);
		IEnumerable<Anime> FilterAnime(string AnimeTitle);
	}
}
