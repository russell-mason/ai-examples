# Getting Started

The following assumes you're using Visual Studio, so adjust accordingly for other IDEs.

### This is the entry point for examples relating to tokenization, vectors, embeddings, and Vector Databases

**N.B.** Ensure you set **VectorData.ConsoleApp** as the startup project.

- This is for experimental purposes only, used to aid in the learning and exploration of AI through vector search
- All examples are listed in Program.cs
- By default all examples are commented out, e.g.

```csharp
//await host.ExecuteExampleAsync<TokenizerExample>();
```

- In order to run an example just uncomment that line, e.g.

```csharp
await host.ExecuteExampleAsync<TokenizerExample>();
```

- Examples are roughly grouped into sections based on the resources they use. 
  This is intended to indicate whether local services need to be running, and whether there'll be a cost associated with 
  running the example or not

The extension method **ExecuteExampleAsync** is used to ensure executing an example is as terse as possible. 
This extension method can be found in the **AIExamples.Shared** project.

### Configuration
 
There are a mix of examples that use Azure AI Foundry, Azure CosmosDB, and Qdrant to demonstate diffenrent aspects
of vector search as it relates to AI, and RAG.

Configuration for all examples is located in the **AIExamples.Shared** project, in the **appsettings.json** file.

See the **README.md** file in **AIExamples.Shared** for more details.

Each example specifically selects a source project and settings, so if you want to change the model, you'll also need 
to change the selection within any given example.

See the **README.md** file in **VectorData.Examples** for more details.

### Qdrant

You can run a local instance of Qdrant using docker.  
Run ```docker compose up -d``` from within the VectorData.ConsoleApp\Docker\Qdrant directory
