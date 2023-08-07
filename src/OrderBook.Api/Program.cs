using OrderBook.Application;
using OrderBook.Infrastructure;
using OrderBook.Infrastructure.Interfaces;
using System.Text.Json.Serialization;
using OrderBook.Domain;
using FluentValidation;
using OrderBook.Api.Requests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); }); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<IValidator<CalculateOptimalStrategyRequest>, CalculateOptimalStrategyRequestValidator>();
builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddScoped<IDataReaderService, DataReaderService>();

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
