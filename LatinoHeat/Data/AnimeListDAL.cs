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

		public IEnumerable<Anime> FilterAnime(string? AnimeTitle)
		{
			if (string.IsNullOrEmpty(AnimeTitle))
				return GetAnime();

			return GetAnime().Where(m => !string.IsNullOrEmpty(m.Title)
				&& m.Title.ToLower().Contains(AnimeTitle.ToLower())).ToList();
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
			//remove this anime's reactions first (no FK cascade exists)
			var reactions = _animeDbContext.AnimeReactions.Where(r => r.AnimeId == anime.Id);
			_animeDbContext.AnimeReactions.RemoveRange(reactions);
			_animeDbContext.Animes.Remove(anime);
			_animeDbContext.SaveChanges();
        }
		public bool AnimeExists(int id)
		{
			return _animeDbContext.Animes.Any(e => e.Id == id);
        }

        public void ToggleReaction(int animeId, string userId, bool isLike)
        {
            //Check if the reaction already exists
			var existingReaction = _animeDbContext.AnimeReactions.FirstOrDefault(r => r.AnimeId == animeId && r.UserId == userId);
			//if there isnt a reaction, create one
			if (existingReaction == null)
			{
				var newReaction = new AnimeReaction();
				newReaction.AnimeId = animeId;
				newReaction.UserId = userId;
				newReaction.IsLiked = isLike;
				_animeDbContext.AnimeReactions.Add(newReaction);
				_animeDbContext.SaveChanges();
            }
			else if (existingReaction.IsLiked == isLike)
			{
				//clicking the same button again removes the reaction (un-vote)
				_animeDbContext.AnimeReactions.Remove(existingReaction);
				_animeDbContext.SaveChanges();
            }
			else
			{
				//clicking the opposite button switches the reaction
				existingReaction.IsLiked = isLike;
				_animeDbContext.SaveChanges();
            }
        }

        public IEnumerable<AnimeBrowseViewModel> GetAnimeWithReactions(string? currentUserId, string? searchString = null)
        {
            var result = new List<AnimeBrowseViewModel>();

            foreach (var anime in FilterAnime(searchString))
            {
                var reactions = _animeDbContext.AnimeReactions.Where(r => r.AnimeId == anime.Id);

                var viewModel = new AnimeBrowseViewModel();
                viewModel.Anime = anime;
                viewModel.TotalLikes = reactions.Count(r => r.IsLiked);
                viewModel.TotalDislikes = reactions.Count(r => !r.IsLiked);

                //the current user's reaction: true = liked, false = disliked, null = no vote
                if (!string.IsNullOrEmpty(currentUserId))
                {
                    var userReaction = reactions.FirstOrDefault(r => r.UserId == currentUserId);
                    if (userReaction != null)
                    {
                        viewModel.UserLiked = userReaction.IsLiked;
                    }
                }

                result.Add(viewModel);
            }

            return result;
        }
    }
}
