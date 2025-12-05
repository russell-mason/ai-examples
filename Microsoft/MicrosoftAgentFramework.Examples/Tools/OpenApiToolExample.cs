namespace MicrosoftAgentFramework.Examples.Tools;

// This is not intended to be realistic, i.e. it's an overly simplistic API, has no authentication or error handling etc.
// This is explicitly designed to demonstrate the prompt is able to pick up the OpenAPI schema and orchestrate multiple function calls
// N.B. The Open API tool errors when receiving a 404 which you could use to determine if the user exists or not,
//      which is why there is an exists API.

// This example creates a Persistent Agent in AI Foundry, so would not be able to get access to a locally running version of the API
// (see the OpenApiWebApi project). You must, therefore, deploy this via an Azure App Service, for example. This loads the OpenAPI
// schema via an HTTP call to the actual API.
// The Persistent Agent will be cleaned up when the example completes.

// Make sure you change the OpenApi:Endpoint in appsettings.json in the AIExamples.Shared project (or user secrets).

/// <summary>
/// Demonstrates using a persistent agent with an OpenAPI schema tool that infers a set of functions that can then be used 
/// automatically given the context of a prompt.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.Tools)]
[ExampleCategory(Category.OpenAPI)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.GPT4Mini)]
[ExampleResourceUse(Resource.HTTP)]
[ExampleCostEstimate(0.004)]
public class OpenApiToolExample(AzureAIFoundrySettings azureSettings, OpenApiSettings openApiSettings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = azureSettings.Projects.Default;

        var httpClient = new HttpClient();

        // URL from config, or local file
        var specification = BinaryData.FromBytes(await httpClient.GetByteArrayAsync(openApiSettings.Endpoint));
        
        var openApiToolDefinition = new OpenApiToolDefinition("manage_users",
                                                              "Manages users, allowing them to be created and retrieved.",
                                                              specification,
                                                              new OpenApiAnonymousAuthDetails());

        var client = new PersistentAgentsClient(project.Endpoint, new DefaultAzureCredential());

        const string instructions = """
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
                                    """;

        PersistentAgent agent = await client.Administration.CreateAgentAsync(project.DeployedModels.Default,
                                                                             "Open API Tool Calling Agent",
                                                                             "Calls web API to create test users",
                                                                             instructions,
                                                                             [openApiToolDefinition]);

        PersistentAgentThread thread = await client.Threads.CreateThreadAsync();

        const string prompt =
            "Create a new user account for 'Bob Smith' with a username of 'bob_smith_001', and an email of 'bob.smith@example.com'.";

        await client.Messages.CreateMessageAsync(thread.Id, MessageRole.User, prompt);

        ThreadRun run = await client.Runs.CreateRunAsync(thread, agent);

        do
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(500));

            run = await client.Runs.GetRunAsync(thread.Id, run.Id);
        } while (run.Status == RunStatus.Queued || run.Status == RunStatus.InProgress || run.Status == RunStatus.RequiresAction);

        if (run.Status == RunStatus.Failed)
        {
            Console.WriteError(run.LastError.Message);
        }
        else
        {
            Pageable<PersistentThreadMessage> messages = client.Messages.GetMessages(thread.Id, order: ListSortOrder.Ascending);

            foreach (var threadMessage in messages)
            {
                foreach (var content in threadMessage.ContentItems)
                {
                    switch (content)
                    {
                        case MessageTextContent textItem:
                            Console.WriteTitle($"[{threadMessage.Role}]");
                            Console.WriteLine($"{textItem.Text}");

                            break;
                    }
                }
            }
        }

        await client.Threads.DeleteThreadAsync(thread.Id);
        await client.Administration.DeleteAgentAsync(agent.Id);
    }
}
