var host = Host.CreateDefaultBuilder()
               .ConfigureAzureAIFoundry()
               .RegisterExamples()
               .Build();



// ---------------------------------------------------------------------------------------
// Tokenizers
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<TikTokenTokenizerExample>();



// ---------------------------------------------------------------------------------------
// Embeddings - From Azure Deployments
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<TextEmbedding3SmallExample>();
//await host.ExecuteExampleAsync<TextEmbeddingAda002Example>();



// ---------------------------------------------------------------------------------------
// Vector Search - From In-Memory
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<InMemoryVectorSearchExample>();
