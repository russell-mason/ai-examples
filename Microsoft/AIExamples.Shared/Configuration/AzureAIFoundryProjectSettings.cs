namespace AIExamples.Shared.Configuration;

public class AzureAIFoundryProjectSettings
{
    public string Key { get; set; } = string.Empty;

    public string Endpoint { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public string OpenAIEndpoint { get; set; } = string.Empty;

    public AzureAIFoundryModelDeploymentSettings DeployedModels { get; set; } = new();
}
