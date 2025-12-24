using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Movie.Api.WebApi.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Movie API",
                    Version = "v1"
                });
            });

            return services;
        }
    }
}
