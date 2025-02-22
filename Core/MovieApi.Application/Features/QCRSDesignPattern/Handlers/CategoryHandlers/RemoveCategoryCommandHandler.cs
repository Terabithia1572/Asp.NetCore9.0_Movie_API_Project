﻿using MovieApi.Application.Features.QCRSDesignPattern.Commands.CategoryCommands;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.QCRSDesignPattern.Handlers.CategoryHandlers
{
    public class RemoveCategoryCommandHandler
    {
        private readonly MovieContext _context;

        public RemoveCategoryCommandHandler(MovieContext context)
        {
            _context = context;
        }
        public async Task Handle(RemoveCategoryCommand removeCategoryCommand)
        {
            var values=_context.Categories.Find(removeCategoryCommand.CategoryID);
            _context.Categories.Remove(values);
            await _context.SaveChangesAsync();

        }
    }
}
