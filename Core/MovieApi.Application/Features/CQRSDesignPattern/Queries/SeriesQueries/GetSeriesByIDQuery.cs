using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Queries.SeriesQueries
{
    public class GetSeriesByIDQuery
    {
        public int SeriesID { get; set; }

        public GetSeriesByIDQuery(int seriesID)
        {
            SeriesID = seriesID;
        }
    }
}
