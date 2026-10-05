namespace MovieApi.Application.Features.CQRSDesignPattern.Queries.UserFavoriteQueries
{
    public class GetUserFavoriteMoviesQuery
    {
        public string UserId { get; set; }

        public GetUserFavoriteMoviesQuery(string userId)
        {
            UserId = userId;
        }
    }
}
