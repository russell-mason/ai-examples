namespace MicrosoftAgentFramework.Examples.Foundation.Models;

public class UserPreferences
{
    public string? Name { get; set; }

    public List<string> Likes { get; set; } = [];

    public List<string> Dislikes { get; set; } = [];
}
