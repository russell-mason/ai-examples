var host = Host.CreateDefaultBuilder()
               .RegisterExamples()
               .Build();

await host.ExecuteExampleAsync<AndGateExample>();
