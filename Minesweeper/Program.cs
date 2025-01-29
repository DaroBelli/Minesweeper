using Database;
using Microsoft.EntityFrameworkCore;
using Minesweeper.Interfaces;
using Minesweeper.Repositories.Minesweeper;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMinesweeperRepository, MinesweeperRepository>();

builder.Services.AddDbContext<MinesweeperContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("postgres")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
