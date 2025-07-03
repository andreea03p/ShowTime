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

// cand in cod aduce o interfata foloseste generic repo de artist/..
builder.Services.AddTransient<IGenericRepository<Artist>, GenericRepository<Artist>>();
builder.Services.AddTransient<IGenericRepository<Festival>, GenericRepository<Festival>>();

builder.Services.AddTransient<IArtistService, ArtistService>();
builder.Services.AddTransient<IFestivalService, FestivalService>();

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.MapControllers();

app.Run();