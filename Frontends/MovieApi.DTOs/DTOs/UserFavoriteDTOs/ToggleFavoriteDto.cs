namespace MovieApi.DTOs.DTOs.UserFavoriteDTOs
{
    public class ToggleFavoriteDto
    {
        public string UserId { get; set; } = null!;
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
    }
}
