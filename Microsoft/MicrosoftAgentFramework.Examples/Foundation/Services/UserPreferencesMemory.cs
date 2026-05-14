namespace MicrosoftAgentFramework.Examples.Foundation.Services;

public class UserPreferencesMemory(IChatClient chatClient, 
                                   DataStore<UserPreferences> dataStore) : AIContextProvider
{
    public const string UserIdStateKey = "userId";

    protected override ValueTask<AIContext> ProvideAIContextAsync(InvokingContext context,
                                                                  CancellationToken cancellationToken = default)
    {
        var aiContext = new AIContext();
        var builder = new StringBuilder();

        var userPreferences = GetUserPreferences(context.Session);

        if (userPreferences.Name is not null)
        {
            builder.AppendLine($"The user's name is {userPreferences.Name}. Address them by name only when appropriate.");
        }

        if (userPreferences.Likes.Count > 0)
        {
            builder.AppendLine($"The user likes {string.Join("|", userPreferences.Likes)}. Each item is separated by |.");
        }

        if (userPreferences.Dislikes.Count > 0)
        {
            builder.AppendLine($"The user dislikes {string.Join("|", userPreferences.Dislikes)}. Each item is separated by |");
        }

        aiContext.Instructions = builder.ToString();

        return new ValueTask<AIContext>(aiContext);
    }

    protected override async ValueTask StoreAIContextAsync(InvokedContext context,
                                                           CancellationToken cancellationToken = default)
    {
        var userPreferences = GetUserPreferences(context.Session);

        var messages = context.RequestMessages.ToList();

        if (userPreferences.Name is null)
        {
            var nameResult = await chatClient.GetResponseAsync<UserPreferences>(
                messages,
                new ChatOptions { Instructions = """
                                                 TASK:
                                                 Extract the user's name ONLY if the user states that it's their name.
                                                 DEFINITION:
                                                 - A user's name exists ONLY if the text includes a clear self-identification.
                                                 NON-EXAMPLES (must NOT be treated as names):
                                                 - Names of places, buildings, institutions, or locations
                                                 - Capitalized noun phrases without identity statements
                                                 - Sentences describing visits, events, or actions
                                                 - Proper nouns not explicitly tied to the user's identity
                                                 """ },
                cancellationToken: cancellationToken);

            userPreferences.Name ??= SanitizeValue(nameResult.Result.Name);
        }

        var likesResult = await chatClient.GetResponseAsync<UserPreferences>(
            messages,
            new ChatOptions
            {
                Instructions = """
                               ROLE:
                               You are performing strict sentiment analysis to extract entities.
                               TASK:
                               From the text extract entities ONLY expressed as likes and dislikes.
                               DEFINITIONS (do not deviate):
                               - A LIKE exists ONLY if the text contains an explicit positive evaluation word
                                 directly attached to an entity
                               - A DISLIKE exists ONLY if the text contains an explicit negative evaluation word
                               - Mentions without evaluation are NEUTRAL and MUST be ignored
                               FORBIDDEN:
                               - Do NOT infer sentiment
                               - Do NOT use overall tone
                               - Do NOT guess
                               - Do NOT include neutral entities
                               RESPONSE FORMAT (do not deviate):
                               - Provide a JSON object with two arrays: "Likes" and "Dislikes"
                               - For each entity, preserve the original text, but normalize its casing as follows:
                                 - Use the whole entity for context
                                 - Common nouns (e.g. foods, objects, activities, generic categories) MUST be all lowercase
                                 - Proper nouns (e.g. cities, countries, landmarks, people's names) MUST use standard capitalization
                               EXAMPLES:
                               Text: "I like london, but not birmingham. The washington monument is great."
                               Output: {"Likes": ["London", "Washington Monument"], "Dislikes": ["Birmingham"]}
                               Text: "I like Rice, but not Noodles."
                               Output: {"Likes": ["rice"], "Dislikes": ["noodles"]}
                               """
            },
            cancellationToken: cancellationToken);

        userPreferences.Likes = userPreferences.Likes.Union(likesResult.Result.Likes, StringComparer.OrdinalIgnoreCase).ToList();
        userPreferences.Dislikes = userPreferences.Dislikes.Union(likesResult.Result.Dislikes, StringComparer.OrdinalIgnoreCase).ToList();
    }

    private UserPreferences GetUserPreferences(AgentSession? session)
    {
        if (session == null) throw new InvalidOperationException("Session is required to retrieve user preferences.");

        session.StateBag.TryGetValue<string>(UserIdStateKey, out var key);
            
        if (key == null) throw new InvalidOperationException("User ID is required in session state bag to retrieve user preferences.");

        dataStore.TryGet(key, out var userPreferences);

        if (userPreferences != null) return userPreferences;

        userPreferences = new UserPreferences();
        dataStore.Add(key, userPreferences);

        return userPreferences;
    }

    private static string? SanitizeValue(string? value) =>
        (string.IsNullOrWhiteSpace(value) || value.Equals("null", StringComparison.OrdinalIgnoreCase))
            ? null
            : value.Trim();
}
