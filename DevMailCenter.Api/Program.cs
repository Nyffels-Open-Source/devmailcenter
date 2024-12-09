using System.Text.Json.Serialization;
using DevMailCenter.Api.Middleware;
using DevMailCenter.Core;
using DevMailCenter.External;
using DevMailCenter.Repository;
using DevMailCenter.Logic;
using DevMailCenter.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<DmcContext>(options => options.UseMySQL(
    builder.Configuration.GetConnectionString("Database"), optionsBuilder => optionsBuilder.CommandTimeout(60)));

builder.Services
    .AddScoped<IMailServerRepository, MailServerRepository>()
    .AddScoped<IEmailRepository, EmailRepository>()
    .AddScoped<IEmailLogic, EmailLogic>()
    .AddScoped<ISmtpLogic, SmtpLogic>()
    .AddScoped<IMicrosoftLogic, MicrosoftLogic>()
    .AddScoped<IEmailTransactionRepository, EmailTransactionRepository>()
    .AddScoped<IMicrosoftApi, MicrosoftApi>()
    .AddScoped<IEncryptionLogic, EncryptionLogic>();

builder.Services.AddAuthentication("DmcAuthentication")
    .AddScheme<AuthenticationSchemeOptions, DmcAuthenticationHandler>("DmcAuthentication", null);

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapOpenApi();
app.MapScalarApiReference(o =>
{
    o.WithTitle("DevMailCenter.Api")
        .WithEndpointPrefix("api/{documentName}")
        .WithTestRequestButton(false)
        .WithModels(false);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();