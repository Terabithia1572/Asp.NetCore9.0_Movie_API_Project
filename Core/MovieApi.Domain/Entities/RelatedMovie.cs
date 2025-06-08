using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Domain.Entities
{
    public class RelatedMovie
    {
        public int RelatedMovieID { get; set; }
        public int MovieID { get; set; }
        public int UserID { get; set; }
        public bool IsWatch { get; set; }
    }
}
