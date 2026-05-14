namespace MicrosoftAgentFramework.Examples.Foundation.Services;

public class UserMemory(IChatClient chatClient,
                        DataStore<User> dataStore) : AIContextProvider
{
    public const string UserIdStateKey = "userId";

    protected override ValueTask<AIContext> ProvideAIContextAsync(InvokingContext context,
                                                                  CancellationToken cancellationToken = default)
    {
        var user = GetUser(context.Session);

        var aiContext = new AIContext
        {
            Instructions = user.Name is null
                                ? "Ask the user for their name and politely decline to answer any questions until they provide it." +
                                  "Example: Can I please take your name? I can't answer questions until then. Thanks you."
                                : $"The user's name is {user.Name}."
        };

        return new ValueTask<AIContext>(aiContext);
    }

    protected override async ValueTask StoreAIContextAsync(InvokedContext context,
                                                           CancellationToken cancellationToken = default)
    {
        var user = GetUser(context.Session);

        if (user.Name is null)
        {
            var result = await chatClient.GetResponseAsync<User>(
                context.RequestMessages,
                new ChatOptions { Instructions = "Extract the user's name if present." },
                cancellationToken: cancellationToken);

            user.Name ??= result.Result.Name;
        }
    }

    private User GetUser(AgentSession? session)
    {
        if (session == null) throw new InvalidOperationException("Session is required to retrieve user.");

        session.StateBag.TryGetValue<string>(UserIdStateKey, out var key);

        if (key == null) throw new InvalidOperationException("User ID is required in session state bag to retrieve user.");

        dataStore.TryGet(key, out var user);

        if (user != null) return user;

        user = new User();
        dataStore.Add(key, user);

        return user;
    }
}
