using EventSourcing.API.EventStores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddEventStore(builder.Configuration);


builder.Services.AddSingleton<ProductStream>(); //Events will be reflected to the database with ProductStream. //ProductStream ile beraber Event'lerin veri tabanýna yansýmasý saðlanacak.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
