namespace MicrosoftSemanticKernel.Examples.Plugins.Native;

/// <summary>
/// Demonstrates how to create a custom plugin that provides some data that can then be included in prompt queries.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class PersonalDetailsPluginExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        kernel.Plugins.AddFromType<PersonalDetailsPlugin>();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var promptSettings = new OpenAIPromptExecutionSettings
                             {
                                 FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                             };

        var chatHistory = new ChatHistory();

        const string prompt1 = "What is the telephone number for Bob Smith, and when is he available?";
        chatHistory.AddUserMessage(prompt1);

        var response1 = await chatCompletionService.GetChatMessageContentAsync(chatHistory, promptSettings, kernel);
        chatHistory.AddAssistantMessage(response1.Content!);

        const string prompt2 = "What is the area code associated with that number?";
        chatHistory.AddUserMessage(prompt2);

        var response2 = await chatCompletionService.GetChatMessageContentAsync(chatHistory, promptSettings, kernel);

        Console.WriteLine(response1.Content);
        Console.WriteLine();
        Console.WriteLine(response2.Content);
    }
}
