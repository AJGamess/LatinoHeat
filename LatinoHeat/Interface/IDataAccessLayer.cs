using LatinoHeat.Models;

namespace LatinoHeat.Interface
{
	public interface IDataAccessLayer
	{
		IEnumerable<Anime> GetAnime();
		void CreateAnime(Anime anime);
		Anime? GetAnime(int id);
		void UpdateUpdate(Anime anime);
		IEnumerable<Anime> FilterAnime(string AnimeName);
	}
}
