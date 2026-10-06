var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

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
