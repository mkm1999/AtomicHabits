using Application.Interfaces.Context;
using Application.ToDoServices;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string ConnectionString = builder.Configuration.GetValue<string>("ConnectionStrings:SqlServer");
builder.Services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(ConnectionString));
builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
builder.Services.AddTransient<IToDoService, ToDoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
