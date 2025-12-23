# Getting Started

These examples can be executed via the **VectorData.ConsoleApp** project.

### Intent

These examples are for learning purposes, and intended to show the features provided by various vector creation methods.

Examples attempt to be self-contained. While this results in duplication of code, the idea is to be able to go to an
example and just see the code required to complete that task. There are some exceptions to this, such as injecting
services to provide broader and more realistic scenarios, but these should be fairly self-evident from the context.

All cost estimates are in USD. The minimum estimates is $0.001, i.e. a tenth of a cent. Some examples only use a few
tokens so would be less, but for the sake of simplicity a tenth of a cent seems reasonable. Estimates are purely an
indicator of scale, i.e. a fraction of a cent, a few cents, etc. They are not intended to be accurate based on actual token
usage.

# Examples

### Tokenizers

-   **TikTokenTokenizerExample**  
    Demonstrates turning text into a series of tokens using the TikToken tokenizer.

-   **BertTokenizerExample**  
    Demonstrates turning text into a series of tokens using the BERT tokenizer.

### Embeddings

-   **TextEmbedding3SmallExample**  
    Demonstrates using a text-embedding-3-small embedding client to take text and turn it into a vector/embedding
    capable for use with a vector store.  
    Model: text-embedding-3-small

-   **TextEmbeddingAda002Example**  
    Demonstrates using a text-embedding-ada-002 embedding client to take text and turn it into a vector/embedding
    capable for use with a vector store.  
    Model: text-embedding-ada-002

-   **PlotEmbeddingExample**  
    Demonstrates reducing 512-dimensional embeddings to 2-dimensional coordinates that can be used to produce a chart.
    e.g. In Excel, the output of this example can be pasted as csv data, a scatter chart created, and data labels added.
    You can then see a very simplistic mapping of the similarity between the words.  
    **N.B.** For visualization purposes only.  
    Model: text-embedding-3-small

### Persistence

**In-Memory**

-   **InMemoryVectorSearchExample**  
    Demonstrates using a simple List\<T\> to store a series of embeddings that can then be used to perform a
    similarity text search.  
    Model: text-embedding-3-small

**Cosmos DB**

-   **CosmosEnsureTestDatabaseAndContainerExistExample**  
    Demonstrates minimal code to create a Cosmos DB database and container. Ensures the connection is correct and working.

-   **CosmosStoreAndRetrieveTestEmbeddingsExample**  
    Demonstrates generating a couple of embeddings from text, storing them, then retrieving them again. Ensures simple
    save/load is working correctly.  
    Model: text-embedding-3-small

-   **CosmosStoreNewsHeadlinesEmbeddingsExample**  
    Demonstrates taking a set of news headlines from a JSON file, creating typed objects, adding embeddings, and saving
    them to the Cosmos DB vector store. This provides permanent storage. This is intended to be the first part of an
    example that can then be used to perform vector searches against existing data.  
    **N.B.** Performing a vector search against this data is done in **CosmosVectorSearchExample.cs**  
    Model: text-embedding-3-small

-   **CosmosVectorSearchExample**  
    Demonstrates creating an embedding from search text, then running a vector search over a set of embedding previously
    stored in the Cosmos DB vector store. The search results represent the most similar matches, and include the original
    typed objects.  
    **N.B.** This relies on data previously stored via **CosmosStoreNewsHeadlinesEmbeddingsExample.cs**  
    Model: text-embedding-3-small

**Qdrant**

-   **QdrantEnsureTestCollectionExistExample**  
    Demonstrates minimal code to create a collection. Ensures the connection is correct and working.

-   **QdrantStoreNewsHeadlinesEmbeddingsExample**  
    Demonstrates taking a set of news headlines from a JSON file, creating typed objects, adding embeddings, and saving
    them to the Qdrant vector store. This provides permanent storage. This is intended to be the first part of an
    example that can then be used to perform vector searches against existing data.  
    **N.B.** Performing a vector search against this data is done in **QdrantVectorSearchExample.cs**  
    Model: text-embedding-3-small

-   **QdrantVectorSearchExample**  
    Demonstrates creating an embedding from search text, then running a vector search over a set of embedding previously
    stored in the Qdrant vector store. The search results represent the most similar matches, and include the original
    typed objects.  
    **N.B.** This relies on data previously stored via **QdrantStoreNewsHeadlinesEmbeddingsExample.cs**  
    Model: text-embedding-3-small

---

Return to the repository [README](../../) file
