using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WeBoard.Server.Configuration;
using WeBoard.Server.Persistence.Data;
using WeBoard.Server.Persistence.Entities;

var builder = WebApplication.CreateBuilder(args);

var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
builder.Services.AddDbContext<WeBoardContext>(options =>
    options.UseMongoDB(mongoDbSettings!.DefaultConnection, mongoDbSettings.DefaultDatabaseName));


builder.Services.AddOpenApi();

builder.Services.AddControllers();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference("scalar");
    app.MapOpenApi();   
}


app.MapControllers();
app.Run();
