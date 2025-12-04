namespace MicrosoftAgentFramework.Examples.Foundation;

public class TranscribeAudioExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.ForDeployedModel(nameof(AzureAIFoundryModelDeploymentSettings.Whisper));

        var openAIClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new AzureKeyCredential(project.ApiKey));
        var audioClient = openAIClient.GetAudioClient(project.DeployedModels.Whisper);

        var result = await audioClient.TranscribeAudioAsync(@".\Foundation\SourceAudio\WinstonChurchillNews.mp3");

        Console.WriteLine(result.Value.Text);
        Console.WriteLine();
    }
}
