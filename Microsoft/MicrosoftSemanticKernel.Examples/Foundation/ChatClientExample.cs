namespace MicrosoftSemanticKernel.Examples.Foundation;

/// <summary>
/// Demonstrates obtaining a chat client from the kernel, and getting a simple response from a prompt.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleCostEstimate(0.001)]
public class ChatClientExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.Default;

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatClient(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatClient>();

        const string prompt = """
                              You are an AI translation service that translates English to French.
                              Your role is to accept English text and echo back that text in French.
                              Once you have given the response please do not make any further statements, or ask any follow up questions.
                              Hello, How do I get to the library?
                              """;

        var response = await chatCompletionService.GetResponseAsync(prompt);

        Console.WriteLine(response.Text);
    }
}
