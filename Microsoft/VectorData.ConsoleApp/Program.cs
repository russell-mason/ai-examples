var host = Host.CreateDefaultBuilder()
               .ConfigureAzureAIFoundry()
               .ConfigureLMStudioAI()
               .ConfigureCosmosDB()
               .ConfigureQdrant()
               .RegisterExamples()
               .Build();



// ---------------------------------------------------------------------------------------
// Tokenizers
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<TikTokenTokenizerExample>();
//await host.ExecuteExampleAsync<BertTokenizerExample>();



// ---------------------------------------------------------------------------------------
// Embeddings - From Azure Deployments
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<TextEmbedding3SmallExample>();
//await host.ExecuteExampleAsync<TextEmbeddingAda002Example>();
//await host.ExecuteExampleAsync<PlotEmbeddingExample>();



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



// ---------------------------------------------------------------------------------------
// Vector Search - From Qdrant - Docker
// ---------------------------------------------------------------------------------------

//await host.ExecuteExampleAsync<QdrantEnsureTestCollectionExistExample>();
//await host.ExecuteExampleAsync<QdrantStoreNewsHeadlinesEmbeddingsExample>();  // <┐
//await host.ExecuteExampleAsync<QdrantVectorSearchExample>();                  //  ^ Relies on