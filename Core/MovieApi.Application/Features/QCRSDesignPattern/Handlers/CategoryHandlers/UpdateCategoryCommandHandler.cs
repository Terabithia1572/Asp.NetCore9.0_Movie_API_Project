using MovieApi.Application.Features.QCRSDesignPattern.Commands.CategoryCommands;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.QCRSDesignPattern.Handlers.CategoryHandlers
{
    public class UpdateCategoryCommandHandler
    {
        private readonly MovieContext _context;

        public UpdateCategoryCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async void Handle(UpdateCategoryCommand updateCategoryCommand)
        {
            var values =await _context.Categories.FindAsync(updateCategoryCommand.CategoryID);
            values.CategoryName = updateCategoryCommand.CategoryName;
            await _context.SaveChangesAsync();
        }
    }
}
