using System.Text.Json.Serialization;
using DevMailCenter.Core;
using DevMailCenter.Repository;
using DevMailCenter.Logic;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddDbContext<DmcContext>(options => options.UseMySQL(
    builder.Configuration.GetConnectionString("db"), optionsBuilder => optionsBuilder.CommandTimeout(60)));

builder.Services
    .AddScoped<IMailServerRepository, MailServerRepository>()
    .AddScoped<IEmailRepository, EmailRepository>()
    .AddScoped<IEmailLogic, EmailLogic>()
    .AddScoped<ISmtpLogic, SmtpLogic>()
    .AddScoped<IEmailTransactionRepository, EmailTransactionRepository>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapOpenApi();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/openapi/v1.json", "DevMailCenter Api");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();