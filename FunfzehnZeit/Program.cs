using FunfzehnZeit.Models;
using FunfzehnZeit.Services;
using Scalar.AspNetCore;
using FunfzehnZeit.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.Configure<GlobalVariables>(builder.Configuration.GetSection(GlobalVariables.CollectionName));
builder.Services.AddHttpClient<IWebTerminalService, WebTerminalService>();

builder.Services.AddSingleton<IUserSessionService, UserSessionService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
