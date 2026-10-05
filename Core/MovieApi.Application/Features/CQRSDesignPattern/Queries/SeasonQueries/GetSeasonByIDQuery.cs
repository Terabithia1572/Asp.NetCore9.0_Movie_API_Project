namespace MovieApi.Application.Features.CQRSDesignPattern.Queries.SeasonQueries
{
    public class GetSeasonByIDQuery
    {
        public int SeasonID { get; set; }

        public GetSeasonByIDQuery(int id)
        {
            SeasonID = id;
        }
    }
}
