var host = Host.CreateDefaultBuilder()
               .ConfigureAzureAIFoundry()
               .ConfigureGitHubAI()
               .ConfigureOllamaAI()
               .ConfigureLMStudioAI()
               .RegisterExamples()
               .Build();



// ---------------------------------------------------------------------------------------
// Foundation - From Azure Deployments
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<ChatCompletionExample>();
//await host.ExecuteExampleAsync<ChatCompletionStreamingExample>();
//await host.ExecuteExampleAsync<ChatCompletionSettingsExample>();
//await host.ExecuteExampleAsync<ChatCompletionTokenUsageExample>();
//await host.ExecuteExampleAsync<ChatCompletionHistoryExample>();
//await host.ExecuteExampleAsync<ChatCompletionHistoryTruncationReducerExample>();
//await host.ExecuteExampleAsync<ChatCompletionHistorySummarizationReducerExample>();



// ---------------------------------------------------------------------------------------
// Model Providers
// ---------------------------------------------------------------------------------------

//// - From Azure Deployments

//await host.ExecuteExampleAsync<AzureOpenAIExample>();
//await host.ExecuteExampleAsync<AzureGrokExample>();

//// - From GitHub

//await host.ExecuteExampleAsync<GitHubAIInferenceExample>();

//// - From Ollama

//await host.ExecuteExampleAsync<OllamaLocalPhiExample>();
//await host.ExecuteExampleAsync<OllamaStandaloneLocalPhiExample>();

//// - From LM Studio

//await host.ExecuteExampleAsync<LMStudioLocalPhiExample>();
