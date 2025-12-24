using Microsoft.AspNetCore.Builder;

namespace Movie.Api.WebApi.Extensions
{
    public static class SwaggerMiddlewareExtensions
    {
        public static WebApplication UseSwaggerMiddleware(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie API v1");
                    c.RoutePrefix = string.Empty; // root = swagger
                });
            }

            return app;
        }
    }
}
