namespace MovieApi.Application.Features.CQRSDesignPattern.Queries.UserFavoriteQueries
{
    public class IsSeriesFavoritedQuery
    {
        public string UserId { get; set; }
        public int SeriesId { get; set; }

        public IsSeriesFavoritedQuery(string userId, int seriesId)
        {
            UserId = userId;
            SeriesId = seriesId;
        }
    }
}
