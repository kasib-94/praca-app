using System.Reflection;

using Blazored.LocalStorage;

using DB.Domain;
using DB.Utils;

using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

using Radzen;

using Syncfusion.Blazor;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthenticationCore();
builder.Services.AddAuthorization();



builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnectionString")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
        options.AccessDeniedPath = "/access-denied";
    });
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzM0NDI2N0AzMjM2MmUzMDJlMzBacFkzTi9YZ1I2OWQ3eWFiYk1UalNqdUg5VWI3c2dLUngxSzhjVEZ4NlhJPQ==");
builder.Services.AddSyncfusionBlazor();
builder.Services.AddRadzenComponents();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); // Register from the main assembly
    cfg.RegisterServicesFromAssembly(typeof(DB.Modules.Authentication.Register.Register.RegisterHandler).Assembly); // Register from the external assembly
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>)); // Add the validation behavior
});
builder.Services.AddValidatorsFromAssembly(typeof(DB.Modules.Authentication.Register.Register.RegisterHandler).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddBlazoredLocalStorage(config => config.JsonSerializerOptions.WriteIndented = true);
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();



builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
