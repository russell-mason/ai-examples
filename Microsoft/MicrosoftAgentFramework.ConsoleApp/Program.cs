var host = Host.CreateDefaultBuilder()
               .ConfigureAzureAIFoundry()
               .ConfigureLMStudioAI()
               .RegisterExamples()
               .Build();



// ---------------------------------------------------------------------------------------
// Foundation
// ---------------------------------------------------------------------------------------

//// - From Azure Deployments

await host.ExecuteExampleAsync<ChatClientExample>();
await host.ExecuteExampleAsync<AgentFromExistingChatClientExample>();
await host.ExecuteExampleAsync<MultipleAgentsFromExistingChatClientExample>();

await host.ExecuteExampleAsync<AgentChatClientExample>();
await host.ExecuteExampleAsync<AgentChatClientStreamingExample>();
await host.ExecuteExampleAsync<AgentChatClientOptionsExample>();
await host.ExecuteExampleAsync<AgentChatClientTokenUsageExample>();

await host.ExecuteExampleAsync<AgentChatClientThreadExample>();
await host.ExecuteExampleAsync<MessageCountingChatReducerExample>();
await host.ExecuteExampleAsync<SummarizingChatReducerExample>();

await host.ExecuteExampleAsync<AzureModelExample>();
await host.ExecuteExampleAsync<StructuredDataExample>();
await host.ExecuteExampleAsync<StructuredDataFromJsonSchemaExample>();
await host.ExecuteExampleAsync<TranscribeAudioExample>();

// - From LM Studio

await host.ExecuteExampleAsync<LocalModelExample>();
await host.ExecuteExampleAsync<DataContentExample>();
await host.ExecuteExampleAsync<ImageDescriptionExample>();
await host.ExecuteExampleAsync<VehicleDamageAssessmentExample>();
