using Funfzehnzeit.Services;
using FunfzehnZeit.Models;
using FunfzehnZeit.Services;
using Scalar.AspNetCore;
using FunfzehnZeit.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.Configure<GlobalVariables>(builder.Configuration.GetSection(GlobalVariables.CollectionName));
builder.Services.AddHttpClient<IWebTerminalService, WebTerminalService>();

builder.Services.AddScoped<IApplication, Application>();
builder.Services.AddSingleton<IUserSessionService, UserSessionService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var application = services.GetRequiredService<IApplication>();
    application.Start();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
