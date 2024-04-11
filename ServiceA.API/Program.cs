

using Polly;
using Polly.Extensions.Http;
using ServiceA.API;
using System.Diagnostics;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpClient<ProductService>(opt =>
{
    opt.BaseAddress = new Uri("https://localhost:5003/api/products/");
}).AddPolicyHandler(GetAdvanceCircuitBreakerPolicy());

var app = builder.Build();


//Retry Pattern
IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions.HandleTransientHttpError().OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound).WaitAndRetryAsync(5, retryAttempt =>
    {
        Debug.WriteLine($"Retry Count {retryAttempt}:");
        return TimeSpan.FromSeconds(10);
    }, onRetryAsync: onretryAsync);
}
Task onretryAsync(DelegateResult<HttpResponseMessage> arg1, TimeSpan arg2)
{
    Debug.WriteLine($"Request is made again: {arg2.TotalMilliseconds} ");
    return Task.CompletedTask;
}



//Circuit Breaker Pattern
IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(3, TimeSpan.FromSeconds(10), onBreak: (arg1, arg2) =>
    {
        Debug.WriteLine("Circuit Breaker Status => On Break");
    }, onReset: () => {

        Debug.WriteLine("Circuit Breaker Status => On Reset");

    }, onHalfOpen: () => {

        Debug.WriteLine("Circuit Breaker Status => On HalfOpen");
    });
}

IAsyncPolicy<HttpResponseMessage> GetAdvanceCircuitBreakerPolicy()
{
    return HttpPolicyExtensions.HandleTransientHttpError().AdvancedCircuitBreakerAsync(0.1,TimeSpan.FromSeconds(30),3,TimeSpan.FromSeconds(30), onBreak: (arg1, arg2) =>
    {
        Debug.WriteLine("Circuit Breaker Status => On Break");
    }, onReset: () => {

        Debug.WriteLine("Circuit Breaker Status => On Reset");

    }, onHalfOpen: () => {

        Debug.WriteLine("Circuit Breaker Status => On HalfOpen");
    });
}


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
