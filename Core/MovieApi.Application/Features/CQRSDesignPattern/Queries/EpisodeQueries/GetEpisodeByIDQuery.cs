namespace MovieApi.Application.Features.CQRSDesignPattern.Queries.EpisodeQueries
{
    public class GetEpisodeByIDQuery
    {
        public int EpisodeID { get; set; }

        public GetEpisodeByIDQuery(int id)
        {
            EpisodeID = id;
        }
    }
}
