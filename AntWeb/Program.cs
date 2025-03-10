using System.Text;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

using MudBlazor.Services;
using Serilog;

using AntWeb;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .WriteTo.Console());

builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddMudServices();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddControllersWithViews();

// ---  AUTH SETUP  ---
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = new PathString("/Auth/Login");
        options.SlidingExpiration = true;

    })
    .AddJwtBearer(options =>
    {
        // TODO: use AppSettings
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Auth:Jwt:Issuer"],
            ValidAudience = builder.Configuration["Auth:Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Auth:Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapDefaultControllerRoute();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
