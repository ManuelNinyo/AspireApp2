using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var db = builder.AddPostgres("db");

var apiService = builder.AddProject<AspireApp2_ApiService>("apiservice")
    .WithHttpsHealthCheck("/health")
    .WithReference(db)
    .WaitFor(db);

builder.AddProject<AspireApp2_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpsHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);


builder.AddKubernetesPublisher();
builder.AddDockerComposePublisher();
builder.Build().Run();