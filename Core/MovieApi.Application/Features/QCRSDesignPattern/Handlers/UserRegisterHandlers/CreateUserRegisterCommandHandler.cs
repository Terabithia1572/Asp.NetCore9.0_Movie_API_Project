﻿using Microsoft.AspNetCore.Identity;
using MovieApi.Application.Features.QCRSDesignPattern.Commands.UserRegisterCommands;
using MovieApi.Persistence.Context;
using MovieApi.Persistence.Identity;

namespace MovieApi.Application.Features.QCRSDesignPattern.Handlers.UserRegisterHandlers
{
    public class CreateUserRegisterCommandHandler
    {
        private readonly MovieContext _movieContext;
        private readonly UserManager<AppUser> _userManager;

        public CreateUserRegisterCommandHandler(MovieContext movieContext, UserManager<AppUser> userManager)
        {
            _movieContext = movieContext;
            _userManager = userManager;
        }

        public async Task Handle(CreateUserRegisterCommand command)
        {
            var user = new AppUser
            {
                Name = command.Name,
                Surname = command.Surname,
                UserName = command.Username,
                Email = command.Email,
            };
            await _userManager.CreateAsync(user, command.Password); //ikinci parametre şifre yolladık neden 
            //AppUser içinde yollamadık çünkü Hashlanerek gönderiliyor.
        }
    }
}
