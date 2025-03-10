using MudBlazor.Services;
using AntWeb;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
