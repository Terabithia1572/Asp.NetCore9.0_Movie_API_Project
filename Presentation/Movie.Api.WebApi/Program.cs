using Microsoft.OpenApi.Models;
using MovieApi.Application.Features.MediatorDesignPattern.Handlers.TagHandler;
using MovieApi.Application.Features.QCRSDesignPattern.Handlers.CategoryHandlers;
using MovieApi.Application.Features.QCRSDesignPattern.Handlers.MovieHandlers;
using MovieApi.Persistence.Context;
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

//builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())); //Bu satýr .NET'te MediatR kütüphanesini yapýlandýrmak için kullanýlýr.
//MediatR, uygulamada "mediator pattern" (arabulucu deseni) uygulamanýzý saðlar.
//Yani, nesnelerin doðrudan birbirleriyle iletiþim kurmak yerine, bir arabulucu (mediator) üzerinden haberleþmesini saðlar.
//Bu desen, özellikle CQRS (Command Query Responsibility Segregation) gibi yaklaþýmlarda çok yaygýndýr.
//Bu satýr, projenizde MediatR kütüphanesini kullanabilmeniz için gerekli olan servisleri, mevcut assembly içinden otomatik olarak bulup uygulamanýza dahil eder.
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
