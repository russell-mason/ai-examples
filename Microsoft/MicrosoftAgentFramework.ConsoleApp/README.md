# Getting Started

The following assumes you're using Visual Studio, so adjust accordingly for other IDEs.

### This is the entry point for all Microsoft Agent Framework examples

**N.B.** Ensure you set **MicrosoftAgentFramework.ConsoleApp** as the startup project.

-   This is for experimental purposes only, used to aid in the learning and exploration of AI through the Agent Framework
-   All examples are listed in Program.cs
-   By default all examples are commented out, e.g.

```csharp
//await host.ExecuteExampleAsync<ChatClientExample>();
```

-   In order to run an example just uncomment that line, e.g.

```csharp
await host.ExecuteExampleAsync<ChatClientExample>();
```

-   Examples are roughly grouped into sections based on the resources they use.
    This is intended to indicate whether local LLMs need to be running, and whether there'll be a cost associated with
    running the example or not

The extension method **ExecuteExampleAsync** is used to ensure executing an example is as terse as possible.
This extension method can be found in the **AIExamples.Shared** project.

### Configuration

There are a mix of examples that use Azure AI Foundry as the source of LLMs, and local models that run under LM Studio
or Ollama. In theory, all examples could be run locally, but this is for learning purposes, so Azure is a realistic
expectation.

Configuration for all examples is located in the **AIExamples.Shared** project, in the **appsettings.json** file.

See the [README](../AIExamples.Shared/README.md) file in **AIExamples.Shared** for more details.

Each example specifically selects a source project and settings, so if you want to change the model, you'll also need
to change the selection within any given example.

See the [README](../MicrosoftAgentFramework.Examples/README.md) file in **MicrosoftAgentFramework.Examples**
for more details.

---

Return to the repository [README](../../README.md) file
