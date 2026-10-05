namespace MovieApi.Application.Features.CQRSDesignPattern.Queries.SeasonQueries
{
    public class GetSeasonBySeriesIDQuery
    {
        public int SeriesID { get; set; }

        public GetSeasonBySeriesIDQuery(int seriesId)
        {
            SeriesID = seriesId;
        }
    }
}
