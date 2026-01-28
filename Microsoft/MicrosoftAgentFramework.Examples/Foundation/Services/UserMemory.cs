namespace MicrosoftAgentFramework.Examples.Foundation.Services;

public class UserMemory(IChatClient chatClient,
                        JsonElement serializedState,
                        JsonSerializerOptions? jsonSerializerOptions = null) : AIContextProvider
{
    public User User { get; } = serializedState.ValueKind == JsonValueKind.Object
        ? serializedState.Deserialize<User>(jsonSerializerOptions)!
        : new User();

    public override ValueTask<AIContext> InvokingAsync(InvokingContext context, CancellationToken cancellationToken = default)
    {
        var aiContext = new AIContext
                        {
                            Instructions = User.Name is null
                                ? "Ask the user for their name and politely decline to answer any questions until they provide it." +
                                  "Example: Can I please take your name? I can't answer questions until then. Thanks you."
                                : $"The user's name is {User.Name}."
                        };

        return new ValueTask<AIContext>(aiContext);
    }

    public override async ValueTask InvokedAsync(InvokedContext context, CancellationToken cancellationToken = default)
    {
        if (User.Name is null)
        {
            var result = await chatClient.GetResponseAsync<User>(
                context.RequestMessages,
                new ChatOptions { Instructions = "Extract the user's name if present." },
                cancellationToken: cancellationToken);

            User.Name ??= result.Result.Name;
        }
    }

    public override JsonElement Serialize(JsonSerializerOptions? jsonSerializerOptions = null) =>
        JsonSerializer.SerializeToElement(User, jsonSerializerOptions);
}
