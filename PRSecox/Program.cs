using Microsoft.EntityFrameworkCore;
using PRSecox.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
     {
         opt.JsonSerializerOptions.ReferenceHandler =
           System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
     });

void AddJsonOptions(Action<object> value)
{
    throw new NotImplementedException();
}

builder.Services.AddDbContext<PRSDbContext>(
        // lambda
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("PrsConnString"))
        );

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
