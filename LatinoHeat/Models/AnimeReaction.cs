namespace LatinoHeat.Models
{
    public class AnimeReaction
    {
        
        public int Id { get; set; }
        public int AnimeId { get; set; }
        public string? UserId { get; set; }
        public bool IsLiked { get; set; }
    }
}
