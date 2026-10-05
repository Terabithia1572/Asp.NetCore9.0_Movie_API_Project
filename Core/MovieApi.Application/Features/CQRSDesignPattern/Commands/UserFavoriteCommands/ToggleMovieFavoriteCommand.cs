namespace MovieApi.Application.Features.CQRSDesignPattern.Commands.UserFavoriteCommands
{
    public class ToggleMovieFavoriteCommand
    {
        public string UserId { get; set; } = null!;
        public int MovieId { get; set; }
    }
}
