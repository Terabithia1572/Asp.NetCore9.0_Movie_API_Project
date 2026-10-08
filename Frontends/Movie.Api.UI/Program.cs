using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Movie.Api.UI.Services;
using MovieApi.Persistence.Context;
using MovieApi.Persistence.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("MovieApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["MovieApi:BaseUrl"] ?? "http://localhost:5114/api/");
    client.Timeout = TimeSpan.FromSeconds(15);
});
builder.Services.AddScoped<MovieApiClient>();
builder.Services.AddScoped<FlixCatalogService>();
// Reuse the existing Identity store; no migrations, creation or seeding are performed here.
builder.Services.AddDbContext<MovieContext>();
builder.Services.AddIdentityCore<AppUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<MovieContext>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "FlixTV.Identity";
        options.LoginPath = "/Login/SignIn";
        options.LogoutPath = "/Login/LogOut";
        options.AccessDeniedPath = "/Error/NotFound404";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStatusCodePagesWithReExecute("/Error/NotFound404");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ✅ Area route (ÖNCE)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// ✅ Default route (SONRA)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movie}/{action=MovieList}/{id?}");

app.Run();
