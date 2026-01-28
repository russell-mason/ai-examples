namespace MicrosoftAgentFramework.Examples.Foundation.Services;

public class UserPreferencesMemory : AIContextProvider
{
    private readonly IChatClient _chatClient;

    public UserPreferencesMemory(IChatClient chatClient,
                                 JsonElement serializedState,
                                 JsonSerializerOptions? jsonSerializerOptions = null)
    {
        _chatClient = chatClient;

        UserPreferences? userPreferences = null;

        if (serializedState.ValueKind == JsonValueKind.Object)
        {
            userPreferences = serializedState.Deserialize<UserPreferences>(jsonSerializerOptions)!;
        }

        UserPreferences = userPreferences ?? new UserPreferences();
    }

    public UserPreferences UserPreferences { get; }

    public override ValueTask<AIContext> InvokingAsync(InvokingContext context, CancellationToken cancellationToken = default)
    {
        var aiContext = new AIContext();
        var builder = new StringBuilder();

        if (UserPreferences.Name is not null)
        {
            builder.AppendLine($"The user's name is {UserPreferences.Name}. Address them by name only when appropriate.");
        }

        if (UserPreferences.Likes.Count > 0)
        {
            builder.AppendLine($"The user likes {string.Join("|", UserPreferences.Likes)}. Each item is separated by |.");
        }

        if (UserPreferences.Dislikes.Count > 0)
        {
            builder.AppendLine($"The user dislikes {string.Join("|", UserPreferences.Dislikes)}. Each item is separated by |");
        }

        aiContext.Instructions = builder.ToString();

        return new ValueTask<AIContext>(aiContext);
    }

    public override async ValueTask InvokedAsync(InvokedContext context, CancellationToken cancellationToken = default)
    {
        var messages = context.RequestMessages.ToList();

        if (UserPreferences.Name is null)
        {
            var nameResult = await _chatClient.GetResponseAsync<UserPreferences>(
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

            UserPreferences.Name ??= SanitizeValue(nameResult.Result.Name);
        }

        var likesResult = await _chatClient.GetResponseAsync<UserPreferences>(
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

        UserPreferences.Likes = UserPreferences.Likes.Union(likesResult.Result.Likes, StringComparer.OrdinalIgnoreCase).ToList();
        UserPreferences.Dislikes = UserPreferences.Dislikes.Union(likesResult.Result.Dislikes, StringComparer.OrdinalIgnoreCase).ToList();
    }

    private static string? SanitizeValue(string? value) =>
        (string.IsNullOrWhiteSpace(value) || value.Equals("null", StringComparison.OrdinalIgnoreCase))
            ? null
            : value.Trim();

    public override JsonElement Serialize(JsonSerializerOptions? jsonSerializerOptions = null) =>
        JsonSerializer.SerializeToElement(UserPreferences, jsonSerializerOptions);
}
