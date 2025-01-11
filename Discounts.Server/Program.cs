using Discounts.Server.Controllers;
using Discounts.Server.DataAccess;
using Discounts.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddRedisClient(connectionName: "redis");

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddScoped<ICodesRepository, CodesRepository>();
builder.Services.AddScoped<IDiscountsService, DiscountsService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountsController>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
