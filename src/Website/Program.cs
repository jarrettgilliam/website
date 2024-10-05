using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Website.Interfaces;
using Website.Models;
using Website.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<AppSecrets>()
    .BindConfiguration(nameof(AppSecrets))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddSingleton<IReCaptchaService, ReCaptchaService>()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddHttpClient();

builder.Services.AddControllers();

builder.Services.AddHsts(
    options =>
    {
        options.Preload = true;
        options.IncludeSubDomains = false;
        options.MaxAge = TimeSpan.FromDays(365);
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Run();