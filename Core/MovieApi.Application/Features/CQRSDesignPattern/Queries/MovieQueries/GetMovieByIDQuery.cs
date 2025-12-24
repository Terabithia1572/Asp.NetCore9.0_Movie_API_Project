using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Queries.MovieQueries
{
    public class GetMovieByIDQuery
    {
        public GetMovieByIDQuery(int movieID)
        {
            MovieID = movieID;
        }

        public int MovieID { get; set; } //Film ID
      
    }
}

