namespace LatinoHeat.Models
{
    public class AnimeBrowseViewModel
    {
        public Anime? Anime { get; set; }
        public int TotalLikes { get; set; }
        public int TotalDislikes { get; set; }
        public bool? UserLiked { get; set; } = null;

    }
}
