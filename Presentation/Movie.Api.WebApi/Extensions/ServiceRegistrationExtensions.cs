using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.CategoryHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.EpisodeHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.MovieHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeasonHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeriesHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.UserFavoriteHandlers;
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
            services.AddScoped<GetSeriesWithCategoryQueryHandler>();

            // Season
            services.AddScoped<GetSeasonQueryHandler>();
            services.AddScoped<GetSeasonByIDQueryHandler>();
            services.AddScoped<GetSeasonBySeriesIDQueryHandler>();
            services.AddScoped<CreateSeasonCommandHandler>();
            services.AddScoped<RemoveSeasonCommandHandler>();
            services.AddScoped<UpdateSeasonCommandHandler>();

            // Episode
            services.AddScoped<GetEpisodeQueryHandler>();
            services.AddScoped<GetEpisodeByIDQueryHandler>();
            services.AddScoped<GetEpisodeBySeasonIDQueryHandler>();
            services.AddScoped<CreateEpisodeCommandHandler>();
            services.AddScoped<RemoveEpisodeCommandHandler>();
            services.AddScoped<UpdateEpisodeCommandHandler>();

            // UserFavorite
            services.AddScoped<GetUserFavoriteMoviesQueryHandler>();
            services.AddScoped<GetUserFavoriteSeriesQueryHandler>();
            services.AddScoped<IsMovieFavoritedQueryHandler>();
            services.AddScoped<IsSeriesFavoritedQueryHandler>();
            services.AddScoped<ToggleMovieFavoriteCommandHandler>();
            services.AddScoped<ToggleSeriesFavoriteCommandHandler>();
            services.AddScoped<RemoveUserFavoriteCommandHandler>();

            // User


            return services;
        }
    }
}
