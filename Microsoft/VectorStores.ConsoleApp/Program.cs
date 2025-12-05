var host = Host.CreateDefaultBuilder()
               .ConfigureAzureAIFoundry()
               .ConfigureCosmosDB()
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



// ---------------------------------------------------------------------------------------
// Vector Search - From Cosmos DB
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<CosmosEnsureTestDatabaseAndContainerExistExample>();
//await host.ExecuteExampleAsync<CosmosStoreAndRetrieveTestEmbeddingsExample>();

//await host.ExecuteExampleAsync<CosmosStoreNewsHeadlinesEmbeddingsExample>();  // <┐ 
//await host.ExecuteExampleAsync<CosmosVectorSearchExample>();                  //  ^ Relies on
