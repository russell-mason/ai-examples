var host = Host.CreateDefaultBuilder()
               .ConfigureAzureAIFoundry()
               .ConfigureGitHubAI()
               .ConfigureOllamaAI()
               .ConfigureLMStudioAI()
               .ConfigureOpenApi()
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



// ---------------------------------------------------------------------------------------
// Multimodal Models - From LM Studio
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<ImageContentExample>();
//await host.ExecuteExampleAsync<ImageDescriptionExample>();



// ---------------------------------------------------------------------------------------
// Audio To Text - From Azure Deployments
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<AudioToTextExample>();



// ---------------------------------------------------------------------------------------
// Structured Data - From Azure Deployments
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<JsonFromChatCompletionExample>();
//await host.ExecuteExampleAsync<ResponseFormatFromInvokePromptExample>();



// ---------------------------------------------------------------------------------------
// Prompt Functions - From Azure Deployments
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<PromptToFunctionExample>();
//await host.ExecuteExampleAsync<JsonFileToFunctionFunctionExample>();
//await host.ExecuteExampleAsync<YamlFileToFunctionFunctionExample>();
//await host.ExecuteExampleAsync<TranslationFunctionExample>();
//await host.ExecuteExampleAsync<FunctionPipelineExample>();



// ---------------------------------------------------------------------------------------
// Plugins - From Azure Deployments
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<NoPluginExample>();

//await host.ExecuteExampleAsync<TimePluginExample>();
//await host.ExecuteExampleAsync<TimePluginUsageExample>();
//await host.ExecuteExampleAsync<FileIOPluginUsageExample>();
//await host.ExecuteExampleAsync<TextPluginUsageExample>();
//await host.ExecuteExampleAsync<HttpPluginUsageExample>();
//await host.ExecuteExampleAsync<ConversationSummaryPluginExample>();

//await host.ExecuteExampleAsync<OpenApiPluginExample>();

//await host.ExecuteExampleAsync<PersonalDetailsPluginExample>();
//await host.ExecuteExampleAsync<TaxCodeServicePluginExample>();



// ---------------------------------------------------------------------------------------
// Embeddings
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<LocalEmbeddingGeneratorExample>();

// - From Azure Deployments

//await host.ExecuteExampleAsync<AzureEmbeddingGeneratorExample>();
//await host.ExecuteExampleAsync<InMemoryVectorSearchExample>();
//await host.ExecuteExampleAsync<InMemoryTextSearchExample>();



// ---------------------------------------------------------------------------------------
// Chunking
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<TextChunkerExample>();

//// - From Azure Deployments

//await host.ExecuteExampleAsync<AzureSemanticChunkerExample>();

//// - From LM Studio

//await host.ExecuteExampleAsync<LocalSemanticChunkerExample>();
