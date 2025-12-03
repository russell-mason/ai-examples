# Getting Started

This library contains configuration, interfaces, and extensions, that apply accross all examples. 

As examples grow, this will allow all configuration to be centralized, requiring little, if no, additional setup 
for new projects.

### Configuration

Configuration settings are stored in **appsettings.json** and wrapped in typed settings classes found in the 
**Configuration** folder. These are bound via HostBuilder extension methods found in **HostBuilderExtensions.cs**.

For Azure AI Foundry projects, there are a set of named models that have been deployed, and a Default. For simplicity, 
examples use the Default value unless they're explicitly showing the use of different models, or when using local 
models to reduce costs.

Local models, loaded via LM Studio or Ollama, will not incur any cost, but unless you have significant processing 
power will take longer to run.

You'll need to update the **appsettings.json** file, or set corresponding user secrets, with your own Azure AI Foundry details in order 
to run the associated examples.

Configuration for local LM Studio or Ollama have been left intact as there's no personal information here and should 
be the same when you install those applications.

Examples use an API key and endpoint to connect to Azure AI Foundry, and URLs for LM Studio or Ollama.

Different Azure AI Foundry projects have been used for different regions, but this is not a requirement and you can use 
a single project so long as all features are available in the selected region.

Where you see **[placeholder]** in the configuration file, replace those values with your configuration.

e.g.

```json
{
    "AzureAIFoundry": {
        "Projects": [
            {
                "Key": "[your-foundry-project-01]",
                "ApiKey": "[your-foundy-project-01-api-key]",
                "Endpoint": "https://[your-foundry-resource-01].services.ai.azure.com/api/projects/[your-foundy-project-01]",
                "OpenAIEndpoint": "https://[your-foundry-resource-01].openai.azure.com",
                "DeployedModels": {
                    "Default": "gpt-4o-mini",
                    "Gpt4oMini": "gpt-4o-mini",
                    "Grok3Mini": "grok-3-mini"
                }
            },
...
        ]
    },
...
    "LMStudioAI": {
        "Endpoint": "http://localhost:1234/v1",
        "Phi4ModelId": "microsoft/phi-4",
        "Qwen3ModelId": "qwen/qwen3-vl-4b"
    },
...
}
```

Here's a template to fill in for users secrets:

```shell
dotnet user-secrets set "AzureAIFoundry:Projects:0:Key" ""
dotnet user-secrets set "AzureAIFoundry:Projects:0:ApiKey" ""
dotnet user-secrets set "AzureAIFoundry:Projects:0:Endpoint" ""
dotnet user-secrets set "AzureAIFoundry:Projects:0:OpenAIEndpoint" ""

dotnet user-secrets set "GitHubAI:ApiKey" ""
```

Note that the UserSecretsId **055603f5-5ef3-43a9-8f59-d02e7e9072f3** has been set against each project.

Examples select a source project and settings, so if you want to change the model, you'll also need to change the 
selection within any given example.

### General structure of an example

All examples implement the IExample interface. This allows for scanning and auto-registration for dependency 
injection. This is achived via the **RegisterExamples** extension method found in **HostBuilderExtensions.cs**.

e.g.

```csharp
public class MyExample(AzureAIFoundrySettings settings, MyService service) : IExample
{
    public async Task ExecuteAsync()
    {
        ...
    }
}
```

All other services, such as **MyService** in the above example, will need to be manually registered in the usual way.


**Attributes**

Attributes can be attached to an example to provide some at-a-glance context, such as whether an Azure LLM, or 
a local LLM, is used, some general categorization, and an indicative estimate of cost.

```csharp
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleResourceUse(Resource.LocalFile)]
[ExampleCostEstimate(0.002)]
public class MyExample(AzureAIFoundrySettings settings, MyService service) : IExample
{
    ...
}
```

Currently these attributes are only used for context, but in the future, these may be used to filter which examples 
get run under different conditions.

### Execution

An additonal extension method, **ExecuteExampleAsync** in **HostExtensions.cs**, allows for a terse one-line 
execution step.

Configuration, registration, and execution, is then a simple matter of a few lines.

```csharp
var host = Host.CreateDefaultBuilder()
               .ConfigureAzureAIFoundry()
               .ConfigureGitHubAI()
               .ConfigureOllamaAI()
               .ConfigureLMStudioAI()
               .RegisterExamples()
               .Build();

await host.ExecuteExampleAsync<MyExample>();
```

### Console extensions

Some simple Console extensions in **ConsoleExtensions.cs** provide output formatting for clarity, separating 
example output, and clearly highlighting unexpected exceptions.
