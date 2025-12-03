namespace AIExamples.Shared.Configuration;

public class AzureAIFoundryProjectSettingsList : List<AzureAIFoundryProjectSettings>
{
    public AzureAIFoundryProjectSettings Default => Count == 0 ? throw new InvalidOperationException("No projects found.") : this[0];

    public AzureAIFoundryProjectSettings this[string key]
    {
        get
        {
            var projects = this.Where(project => project.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).ToList();

            return projects.Count switch
            {
                0 => throw new KeyNotFoundException($"No project found for key '{key}'."),

                > 1 => throw new InvalidOperationException($"Multiple projects found for key '{key}'."),

                _ => projects.First()
            };
        }
    }

    public AzureAIFoundryProjectSettings ForDeployedModel(string modelName)
    {
        var projects = this.Where(project => project.DeployedModels
                                                    .GetType()
                                                    .GetProperties()
                                                    .Any(property => property.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase) &&
                                                                     !string.IsNullOrWhiteSpace(property.GetValue(project.DeployedModels) as string)))
                           .ToList();

        return projects.Count switch
        {
            0 => throw new KeyNotFoundException($"No project found for deployed model '{modelName}'."),

            > 1 => throw new InvalidOperationException($"Multiple projects found for deployed model '{modelName}'."),

            _ => projects.First()
        };
    }
}
