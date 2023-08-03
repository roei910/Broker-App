
using BrokerAppAPI.Models;
using BrokerAppAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Timers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApiContext>
    (opt => {
        opt.UseInMemoryDatabase("BrokerAppDb");
        });

//add background service
builder.Services.AddHostedService<StocksBackgroundTask>();

var app = builder.Build();
app.UseCors(option => option.WithOrigins("http://localhost:51692").AllowAnyHeader().AllowAnyOrigin());

//initialize db
ApiContext.InitializeData(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

