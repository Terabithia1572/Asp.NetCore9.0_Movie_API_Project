using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.CategoryHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.MovieHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeriesHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.UserRegisterHandlers;
using MovieApi.Persistence.Context;
using MovieApi.Persistence.Identity;

namespace Movie.Api.WebApi.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddDbContext<MovieContext>();

            // Category
            services.AddScoped<GetCategoryQueryHandler>();
            services.AddScoped<GetCategoryByIDQueryHandler>();
            services.AddScoped<CreateCategoryCommandHandler>();
            services.AddScoped<RemoveCategoryCommandHandler>();
            services.AddScoped<UpdateCategoryCommandHandler>();

            // Movie
            services.AddScoped<GetMovieQueryHandler>();
            services.AddScoped<GetMovieByIDQueryHandler>();
            services.AddScoped<CreateMovieCommandHandler>();
            services.AddScoped<RemoveMovieCommandHandler>();
            services.AddScoped<UpdateMovieCommandHandler>();
            services.AddScoped<GetMovieWithCategoryQueryHandler>();

            // Series
            services.AddScoped<GetSeriesQueryHandler>();
            services.AddScoped<GetSeriesByIDQueryHandler>();
            services.AddScoped<CreateSeriesCommandHandler>();
            services.AddScoped<RemoveSeriesCommandHandler>();
            services.AddScoped<UpdateSeriesCommandHandler>();

            // User
           

            return services;
        }
    }
}
