using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Domain.Entities
{
    public class Tag
    {
        public int TagID { get; set; } //Etiket ID
        public string TagTitle { get; set; } //Etiket Adı

        public ICollection<MovieTag> MovieTags { get; set; } = new List<MovieTag>();
        public ICollection<SeriesTag> SeriesTags { get; set; } = new List<SeriesTag>();
    }
}
