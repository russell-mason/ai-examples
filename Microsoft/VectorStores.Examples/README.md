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

- **TikTokenTokenizerExample**  
Demonstrates turning text into a series of tokens using the TikToken tokenizer.  

### Embeddings

- **TextEmbedding3SmallExample**  
Demonstrates using a text-embedding-3-small embedding client to take text and turn it into a vector/embedding 
capable for use with a vector store.  

- **TextEmbeddingAda002Example**  
Demonstrates using a text-embedding-ada-002 embedding client to take text and turn it into a vector/embedding 
capable for use with a vector store.  

### Persistence

**In-Memory**

- **InMemoryVectorSearchExample**  
Demonstrates using a simple List\<T\> to store a series of embeddings that can then be used to perform a 
similarity text search.  
