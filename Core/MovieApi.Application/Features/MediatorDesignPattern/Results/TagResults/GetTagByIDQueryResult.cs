using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.MediatorDesignPattern.Results.TagResults
{
    public class GetTagByIDQueryResult
    {
        public int TagID { get; set; } //Etiket ID
        public string TagTitle { get; set; } //Etiket Adı
    }
}
