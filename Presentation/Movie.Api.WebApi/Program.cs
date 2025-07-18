using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using MovieApi.Application.Features.MediatorDesignPattern.Handlers.TagHandler;
using MovieApi.Application.Features.QCRSDesignPattern.Handlers.CategoryHandlers;
using MovieApi.Application.Features.QCRSDesignPattern.Handlers.MovieHandlers;
using MovieApi.Application.Features.QCRSDesignPattern.Handlers.UserRegisterHandlers;
using MovieApi.Persistence.Context;
using MovieApi.Persistence.Identity;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MovieContext>();

builder.Services.AddScoped<GetCategoryQueryHandler>();
builder.Services.AddScoped<GetCategoryByIDQueryHandler>();
builder.Services.AddScoped<CreateCategoryCommandHandler>();
builder.Services.AddScoped<RemoveCategoryCommandHandler>();
builder.Services.AddScoped<UpdateCategoryCommandHandler>();

builder.Services.AddScoped<GetMovieQueryHandler>();
builder.Services.AddScoped<GetMovieByIDQueryHandler>();
builder.Services.AddScoped<CreateMovieCommandHandler>();
builder.Services.AddScoped<RemoveMovieCommandHandler>();
builder.Services.AddScoped<UpdateMovieCommandHandler>();


builder.Services.AddScoped<CreateUserRegisterCommandHandler>();
builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<MovieContext>(); //bu sat�r, Identity k�t�phanesini kullanarak uygulaman�zda kimlik do�rulama ve yetkilendirme i�lemlerini y�netmek i�in gerekli olan servisleri ekler.

//builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())); //Bu sat�r .NET'te MediatR k�t�phanesini yap�land�rmak i�in kullan�l�r.
//MediatR, uygulamada "mediator pattern" (arabulucu deseni) uygulaman�z� sa�lar.
//Yani, nesnelerin do�rudan birbirleriyle ileti�im kurmak yerine, bir arabulucu (mediator) �zerinden haberle�mesini sa�lar.
//Bu desen, �zellikle CQRS (Command Query Responsibility Segregation) gibi yakla��mlarda �ok yayg�nd�r.
//Bu sat�r, projenizde MediatR k�t�phanesini kullanabilmeniz i�in gerekli olan servisleri, mevcut assembly i�inden otomatik olarak bulup uygulaman�za dahil eder.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetTagQueryHandler).Assembly));



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "My Api", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Api V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
