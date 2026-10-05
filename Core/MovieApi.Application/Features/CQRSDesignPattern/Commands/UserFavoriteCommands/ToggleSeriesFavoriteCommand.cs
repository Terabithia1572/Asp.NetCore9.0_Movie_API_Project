namespace MovieApi.Application.Features.CQRSDesignPattern.Commands.UserFavoriteCommands
{
    public class ToggleSeriesFavoriteCommand
    {
        public string UserId { get; set; } = null!;
        public int SeriesId { get; set; }
    }
}
