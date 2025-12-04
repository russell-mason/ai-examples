var host = Host.CreateDefaultBuilder()
               .ConfigureAzureAIFoundry()
               .RegisterExamples()
               .Build();



// ---------------------------------------------------------------------------------------
// Foundation - From Azure Deployments
// ---------------------------------------------------------------------------------------

await host.ExecuteExampleAsync<ChatClientExample>();
await host.ExecuteExampleAsync<AgentFromExistingChatClientExample>();
await host.ExecuteExampleAsync<MultipleAgentsFromExistingChatClientExample>();

await host.ExecuteExampleAsync<AgentChatClientExample>();
await host.ExecuteExampleAsync<AgentChatClientStreamingExample>();
