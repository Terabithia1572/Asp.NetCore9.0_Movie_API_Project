using Microsoft.AspNetCore.Identity;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.UserRegisterHandlers;
using MovieApi.Persistence.Context;
using MovieApi.Persistence.Identity;

namespace Movie.Api.WebApi.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<CreateUserRegisterCommandHandler>();
           services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<MovieContext>(); //bu satır, Identity kütüphanesini kullanarak uygulamanızda kimlik doğrulama ve yetkilendirme işlemlerini yönetmek için gerekli olan servisleri ekler.
            return services;
        }
    }
}
