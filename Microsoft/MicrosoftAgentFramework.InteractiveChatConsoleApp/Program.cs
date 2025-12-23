var host = Host.CreateDefaultBuilder()
               .ConfigureAzureAIFoundry()
               .ConfigureInteractiveChat()
               .Build();

await host.RunAsync();