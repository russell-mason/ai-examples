namespace AIExamples.Shared.Configuration;

public class AzureAIFoundryModelDeploymentSettings
{
    public string Default { get; set; } = string.Empty;

    public string Gpt4Mini { get; set; } = string.Empty;

    public string Grok3Mini { get; set; } = string.Empty;

    public string Whisper { get; set; } = string.Empty;

    public string TextEmbedding3Small { get; set; } = string.Empty;

    public string TextEmbeddingAda002 { get; set; } = string.Empty;
}
