var builder = DistributedApplication.CreateBuilder(args);

var store = builder.AddRedis("redis");

builder.AddProject<Projects.Discounts_Server>("discounts-server")
    .WithReference(store);

builder.Build().Run();
