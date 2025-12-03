namespace MicrosoftSemanticKernel.Examples.Plugins.OpenApi;

// This is not intended to be realistic, i.e. it's an overly simplistic API, has no authentication or error handling etc.
// This is explicitly designed to demonstrate the prompt is able to pick up the OpenAPI schema and orchestrate multiple function calls

// The API for this example must be available. This can be running locally or via a published App Service.
// The web API for this example is available from the OpenApiWebApi project.
// Ensure the correct URL is set against OpenApi:Endpoint in appsettings.json in the AIExamples.Shared project.

/// <summary>
/// Demonstrates how to import an OpenAPI schema that infers a set of functions that can be automatically used via the context 
/// of the prompt.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Plugins)]
[ExampleCategory(Category.OpenAPI)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleResourceUse(Resource.HTTP)]
[ExampleCostEstimate(0.004)]
public class OpenApiPluginExample(AzureAIFoundrySettings azureSettings, OpenApiSettings openApiSettings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = azureSettings.Projects.Default;

        var builder = Kernel.CreateBuilder()
                            .AddAzureOpenAIChatCompletion(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey);

        // Add logging to show the plugin functions being invoked
        builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Information));

        var kernel = builder.Build();

        await kernel.ImportPluginFromOpenApiAsync(openApiSettings.Name, new Uri(openApiSettings.Endpoint));

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var promptSettings = new OpenAIPromptExecutionSettings
                             {
                                 TopP = 1,  // High for "random" password
                                 FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                             };

        const string prompt = """
                              Perform this action. Do not make any further statements or ask any additional questions.
                              Goal:
                              Your task is to create a new user using the details provided.
                              However, a user with that username may already exist. In that case you need to increase the number at the end of the username, 
                              e.g. if the username was bob_smith_001, you would try 'bob_smith_002', 'bob_smith_003' etc., until you find one that doesn't exist. 
                              Step 1: 
                              Check if a user with the username already exists. If they do, repeat until you find a username that does not exist.
                              Step 2:
                              Make up a random password for the user that is 10 characters long and contains numbers, upper and lower case letters, 
                              and remember that password.
                              Step 3:
                              Create the user with those details.
                              Step 4:
                              When the user is created you will get their new ID from the response that is returned.
                              Verify that the user was created by retrieving the user's details using their username.
                              Step 5:
                              Output details of the user that was created, in the format: 
                              User ID: [id]
                              User Name: [user_name]
                              Password: [password]'
                              First Name: [first_name]
                              Last Name: [last_name]
                              
                              Create a new user account for 'Bob Smith' with a username of 'bob_smith_001', and an email of 'bob.smith@example.com'.
                              """;

        var response = await chatCompletionService.GetChatMessageContentAsync(prompt, promptSettings, kernel);

        Console.WriteLine();
        Console.WriteLine(response.Content);
    }
}
