using ShowTime.Components;
using ShowTime.DataAccess;
using Microsoft.EntityFrameworkCore;
using ShowTime.DataAccess.Repositories.Abstraction;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories;
using ShowTime.BusinessLogic.Services;
using ShowTime.BusinessLogic.Abstraction;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("ShowTimeContext");
builder.Services.AddDbContext<ShowTimeDbContext>(options => 
    options.UseSqlServer(connectionString));

// cand in cod aduce o interfata foloseste generic repo de artist
builder.Services.AddTransient<IGenericRepository<Artist>, GenericRepository<Artist>>();

builder.Services.AddTransient<IArtistService, ArtistService>();

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

// Map components with both render modes available
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.MapControllers();

app.Run();