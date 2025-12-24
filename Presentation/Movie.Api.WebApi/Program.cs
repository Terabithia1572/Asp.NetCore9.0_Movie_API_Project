using Movie.Api.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddProjectServices(); //--> Servislerin Dependency Injection konteynerine eklenmesi için kullanýlýr.

builder.Services.AddIdentityServices(); //--> Kimlik doðrulama ve yetkilendirme iþlemlerini yönetmek için gerekli olan servislerin eklenmesi için kullanýlýr.

builder.Services.AddMediatRServices(); //--> MediatR kütüphanesini kullanabilmeniz için gerekli olan servislerin eklenmesi için kullanýlýr.



builder.Services.AddControllers();
builder.Services.AddSwaggerServices(); //--> Swagger belgeleri oluþturmak ve API dokümantasyonu saðlamak için gerekli olan servislerin eklenmesi için kullanýlýr.

var app = builder.Build();

app.UseSwaggerMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
