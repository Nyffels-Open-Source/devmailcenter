using System.Text.Json.Serialization;
using DevMailCenter.Core;
using DevMailCenter.External;
using DevMailCenter.Repository;
using DevMailCenter.Logic;
using Microsoft.EntityFrameworkCore;

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
    .AddScoped<IEmailTransactionRepository, EmailTransactionRepository>()
    .AddScoped<IMicrosoftApi, MicrosoftApi>()
    .AddScoped<IEncryptionLogic, EncryptionLogic>();

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapOpenApi();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/openapi/v1.json", "DevMailCenter Api");
    o.RoutePrefix = "api";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();