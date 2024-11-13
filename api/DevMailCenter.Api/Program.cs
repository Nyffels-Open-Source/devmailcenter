using System.Text.Json.Serialization;
using DevMailCenter.Core;
using DevMailCenter.Logic;
using DevMailCenter.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DmcContext>(options => options.UseMySQL(
    builder.Configuration.GetConnectionString("db"), optionsBuilder => optionsBuilder.CommandTimeout(60)));

builder.Services
    .AddScoped<IMailServerRepository, MailServerRepository>()
    .AddScoped<IMailServerLogic, MailServerLogic>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();