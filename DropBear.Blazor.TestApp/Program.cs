using DropBear.Blazor.Interfaces;
using DropBear.Blazor.Services;
using DropBear.Blazor.TestApp.Components;
using DropBear.Blazor.TestApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IPageAlertService, PageAlertService>();
builder.Services.AddScoped<IDynamicContextMenuService, DynamicContextMenuService>();
builder.Services.AddSingleton<ISnackbarNotificationService, SnackbarNotificationService>();
builder.Services.AddScoped<IModalService, ModalService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
