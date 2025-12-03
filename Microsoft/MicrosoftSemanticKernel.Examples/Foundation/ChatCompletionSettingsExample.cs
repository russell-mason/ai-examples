namespace MicrosoftSemanticKernel.Examples.Foundation;

/// <summary>
/// Demonstrates providing options to the chat completion service, such as a starting prompt and temperature.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class ChatCompletionSettingsExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        const string prompt = "Explain the concept of a sphere";

        var promptSettings = new OpenAIPromptExecutionSettings
                             {
                                 ChatSystemPrompt = "You are an AI Assistant designed for use by very young children.",
                                 Temperature = 0.5, // 0 = Accurate, 1 = Random
                                 MaxTokens = 1000
                             };

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt, promptSettings);

        Console.WriteLine(response.Content);
    }
}
