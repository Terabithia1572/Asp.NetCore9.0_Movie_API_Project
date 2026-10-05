namespace MovieApi.Application.Features.CQRSDesignPattern.Queries.UserFavoriteQueries
{
    public class IsMovieFavoritedQuery
    {
        public string UserId { get; set; }
        public int MovieId { get; set; }

        public IsMovieFavoritedQuery(string userId, int movieId)
        {
            UserId = userId;
            MovieId = movieId;
        }
    }
}
