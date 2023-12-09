using System.Runtime.InteropServices;
using Microsoft.FluentUI.AspNetCore.Components;
using RetroPie.Manager.Web.Client.Layout;
using RetroPie.Manager.Web.Components;
using RetroPie.Manager.Web.Interfaces;
using RetroPie.Manager.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddFluentUIComponents();

builder.Services.AddScoped<IGamingSystemService, GamingSystemService>();

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    builder.Services.AddScoped<ICpuService, CpuWindowsService>();
    builder.Services.AddScoped<IDiskSpaceService, DiskSpaceWindowsService>();
    builder.Services.AddScoped<IMemoryUsageService, MemoryUsageWindowsService>();
}
if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    builder.Services.AddScoped<ICpuService, CpuLinuxService>();
    builder.Services.AddScoped<IDiskSpaceService, DiskSpaceLinuxService>();
    builder.Services.AddScoped<IMemoryUsageService, MemoryUsageLinuxService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(NavMenu).Assembly);

app.Run();
