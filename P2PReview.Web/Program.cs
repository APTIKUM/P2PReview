using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P2PReview.Application.Interfaces;
using P2PReview.Domain.Entities;
using P2PReview.Infrastructure;
using P2PReview.Infrastructure.Services;
using P2PReview.Web.Components;
using P2PReview.Web.Services;
using P2PReview.Web.State;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services
    .AddIdentity<User, IdentityRole>(options => 
    { 
        options.Password.RequiredLength = 8;
    })
    .AddErrorDescriber<RussianIdentityErrorDescriber>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.LoginPath = "/login";

});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<UserContext>();

builder.Services.AddSingleton<AvatarUrlProvider>();

builder.Services.AddControllers();

builder.Services.AddScoped(sp =>
{
    var nav = sp.GetRequiredService<NavigationManager>();

    return new HttpClient
    {
        BaseAddress = new Uri(nav.BaseUri)
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
