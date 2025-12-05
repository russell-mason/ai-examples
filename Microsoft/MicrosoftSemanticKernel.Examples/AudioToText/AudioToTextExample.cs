namespace MicrosoftSemanticKernel.Examples.AudioToText;

#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010

/// <summary>
/// Demonstrates how to read an mps3 audio file and transcribe it to text using the Azure Open AI audio to text service. 
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.AudioToText)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.Whisper)]
[ExampleResourceUse(Resource.LocalFile)]
[ExampleCostEstimate(0.01)]
public class AudioToTextExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.ForDeployedModel(nameof(AzureAIFoundryModelDeploymentSettings.Whisper));

        var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIAudioToText(project.DeployedModels.Default, project.OpenAIEndpoint, project.ApiKey)
                           .Build();

        var audioToTextService = kernel.GetRequiredService<IAudioToTextService>();

        var audioBinary = await File.ReadAllBytesAsync(@".\AudioToText\SourceAudio\WinstonChurchillNews.mp3");
        var audioContent = new AudioContent(audioBinary, "audio/mp3");

        var response = await audioToTextService.GetTextContentAsync(audioContent);

        Console.WriteLine(response.Text);
        Console.WriteLine();
    }
}

#pragma warning restore SKEXP0010
#pragma warning disable SKEXP0001
