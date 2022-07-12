using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Website.Interfaces;
using Website.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IReCaptchaService, ReCaptchaService>();
builder.Services.AddSingleton<IAppSettings, AppSettings>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    LoadEnvFile("secrets.env");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Run();

static void LoadEnvFile(string envFilePath)
{
    using StreamReader sr = File.OpenText(envFilePath);

    while (sr.ReadLine() is { } line)
    {
        if (line.Split('=', 2) is { Length: 2 } parts)
        {
            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}