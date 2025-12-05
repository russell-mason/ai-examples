namespace MicrosoftAgentFramework.Examples.Foundation;

// LM Studio
// https://lmstudio.ai/
// load microsoft/phi-4

/// <summary>
/// Demonstrates using the LM Studio application running locally with a microsoft/phi-4 model.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.LocalLMStudio, AIModel.MSPhi4)]
[ExampleCostEstimate(0.00)]
public class LocalModelExample(LMStudioAISettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var agent = new OpenAIClient(new ApiKeyCredential("NOT_APPLICABLE"), new OpenAIClientOptions { Endpoint = new Uri(settings.Endpoint) })
                    .GetChatClient(settings.Phi4ModelId)
                    .CreateAIAgent();

        const string prompt = "What is your base LLM, including version and cutoff date? Be terse.";

        var response = await agent.RunAsync(prompt);

        Console.WriteLine(response.Text);
    }
}
