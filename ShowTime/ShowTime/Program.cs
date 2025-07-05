using ShowTime.Components;
using ShowTime.DataAccess;
using Microsoft.EntityFrameworkCore;
using ShowTime.DataAccess.Repositories.Abstraction;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories;
using ShowTime.BusinessLogic.Services;
using ShowTime.BusinessLogic.Abstraction;
using Microsoft.AspNetCore.Authentication.Cookies;  

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth-token";
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
        options.ExpireTimeSpan = TimeSpan.FromHours(3);
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("ShowTimeContext");
builder.Services.AddDbContext<ShowTimeDbContext>(options => 
    options.UseSqlServer(connectionString));



// cand in cod aduce o interfata foloseste generic repo de artist/..
builder.Services.AddTransient<IGenericRepository<Artist>, GenericRepository<Artist>>();
builder.Services.AddTransient<IGenericRepository<Festival>, GenericRepository<Festival>>();
builder.Services.AddTransient<IGenericRepository<User>, GenericRepository<User>>();

builder.Services.AddTransient<IArtistService, ArtistService>();
builder.Services.AddTransient<IFestivalService, FestivalService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddServerSideBlazor(options =>
{
    options.DetailedErrors = true;
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.MapControllers();

app.Run();