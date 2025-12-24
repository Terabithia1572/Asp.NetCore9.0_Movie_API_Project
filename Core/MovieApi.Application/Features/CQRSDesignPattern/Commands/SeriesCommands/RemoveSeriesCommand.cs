using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Commands.SeriesCommands
{
    public class RemoveSeriesCommand
    {
        public int SeriesID { get; set; } //Dizi ID

        public RemoveSeriesCommand(int seriesID)
        {
            SeriesID = seriesID;
        }
    }
}
