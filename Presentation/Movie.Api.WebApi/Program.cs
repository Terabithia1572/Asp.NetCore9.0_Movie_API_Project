using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Movie.Api.WebApi.Extensions;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.CategoryHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.MovieHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeriesHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.UserRegisterHandlers;
using MovieApi.Application.Features.MediatorDesignPattern.Handlers.TagHandler;
using MovieApi.Persistence.Context;
using MovieApi.Persistence.Identity;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddProjectServices(); //--> Servislerin Dependency Injection konteynerine eklenmesi için kullanýlýr.

builder.Services.AddIdentityServices(); //--> Kimlik doðrulama ve yetkilendirme iþlemlerini yönetmek için gerekli olan servislerin eklenmesi için kullanýlýr.

builder.Services.AddMediatRServices(); //--> MediatR kütüphanesini kullanabilmeniz için gerekli olan servislerin eklenmesi için kullanýlýr.



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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie API v1");
        c.RoutePrefix = string.Empty; // <-- KRÝTÝK SATIR
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
