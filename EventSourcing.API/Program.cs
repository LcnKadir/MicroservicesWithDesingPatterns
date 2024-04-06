using EventSourcing.API.EventStores;
using EventSourcing.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});



builder.Services.AddEventStore(builder.Configuration);
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<ProductStream>(); //Events will be reflected to the database with ProductStream. //ProductStream ile beraber Event'lerin veri tabanýna yansýmasý saðlanacak.



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
