namespace MovieApi.Application.Features.CQRSDesignPattern.Queries.UserFavoriteQueries
{
    public class GetUserFavoriteSeriesQuery
    {
        public string UserId { get; set; }

        public GetUserFavoriteSeriesQuery(string userId)
        {
            UserId = userId;
        }
    }
}
