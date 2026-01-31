var builder = Host.CreateApplicationBuilder(args);

builder.Services
       .AddSingleton<IHumanResourcesService, HumanResourcesService>()
       .AddMcpServer()
       .WithStdioServerTransport()
       .WithToolsFromAssembly();

await builder.Build().RunAsync();
