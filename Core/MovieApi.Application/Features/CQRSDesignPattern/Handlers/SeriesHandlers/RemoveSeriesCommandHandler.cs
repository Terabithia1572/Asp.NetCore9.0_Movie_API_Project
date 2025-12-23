using MovieApi.Application.Features.CQRSDesignPattern.Commands.SeriesCommands;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeriesHandlers
{
    public class RemoveSeriesCommandHandler
    {
        private readonly MovieContext _context;

        public RemoveSeriesCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task Handle(RemoveSeriesCommand command)
        {
            var series = await _context.Series.FindAsync(command.SeriesID);
            if (series != null)
            {
                _context.Series.Remove(series);
                await _context.SaveChangesAsync();
            }
        }
    }
}
