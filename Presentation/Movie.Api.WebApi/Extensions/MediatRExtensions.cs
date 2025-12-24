using MovieApi.Application.Features.MediatorDesignPattern.Handlers.TagHandler;

namespace Movie.Api.WebApi.Extensions
{
    public static class MediatRExtensions
    {
        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            //builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())); //Bu satır .NET'te MediatR kütüphanesini yapılandırmak için kullanılır.
            //MediatR, uygulamada "mediator pattern" (arabulucu deseni) uygulamanızı sağlar.
            //Yani, nesnelerin doğrudan birbirleriyle iletişim kurmak yerine, bir arabulucu (mediator) üzerinden haberleşmesini sağlar.
            //Bu desen, özellikle CQRS (Command Query Responsibility Segregation) gibi yaklaşımlarda çok yaygındır.
            //Bu satır, projenizde MediatR kütüphanesini kullanabilmeniz için gerekli olan servisleri, mevcut assembly içinden otomatik olarak bulup uygulamanıza dahil eder.
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetTagQueryHandler).Assembly));

            return services;
        }
    }
}
