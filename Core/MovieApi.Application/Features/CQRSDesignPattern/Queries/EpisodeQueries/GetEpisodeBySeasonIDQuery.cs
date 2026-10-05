namespace MovieApi.Application.Features.CQRSDesignPattern.Queries.EpisodeQueries
{
    public class GetEpisodeBySeasonIDQuery
    {
        public int SeasonID { get; set; }

        public GetEpisodeBySeasonIDQuery(int seasonId)
        {
            SeasonID = seasonId;
        }
    }
}
