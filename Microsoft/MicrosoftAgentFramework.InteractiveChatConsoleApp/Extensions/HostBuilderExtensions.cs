namespace MicrosoftAgentFramework.InteractiveChatConsoleApp.Extensions;

public static class HostBuilderExtensions
{
    extension(IHostBuilder builder)
    {
        public IHostBuilder ConfigureInteractiveChat() =>
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<ChatClientAgent>(provider =>
                {
                    var settings = provider.GetRequiredService<AzureAIFoundrySettings>();
                    var project = settings.Projects.Default;

                    const string instructions = """
                                                You are an interactive chat agent.
                                                Respond to the user's questions in a friendly, but concise and informative, manner.
                                                """;

                    var agent = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new ApiKeyCredential(project.ApiKey))
                                .GetChatClient(project.DeployedModels.Default)
                                .CreateAIAgent(instructions);

                    return agent;
                });

                services.AddHostedService<ChatService>();
            });
    }
}
