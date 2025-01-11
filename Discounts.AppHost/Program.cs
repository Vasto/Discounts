var builder = DistributedApplication.CreateBuilder(args);

var store = builder.AddRedis("redis");
store.WithRedisCommander();
store.WithDataVolume(isReadOnly: false);

builder.AddProject<Projects.Discounts_Server>("discounts-server")
    .WithReference(store);

builder.AddProject<Projects.Discounts_Client>("discounts-client");

builder.Build().Run();
