
using BrokerAppAPI.Models;
using BrokerAppAPI.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var mongo = false;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
if (mongo) {
    builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
    builder.Services.AddSingleton<MongoDBService>();
} 
else {
    builder.Services.AddDbContext<ApiContext>
    (opt => {
        opt.UseInMemoryDatabase("BrokerAppDb");
    });
}


/**/

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

