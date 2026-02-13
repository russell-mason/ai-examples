namespace MicrosoftAgentFramework.Examples.Foundation;

/// <summary>
/// Demonstrates how to use an audio client to convert text into an MP3 stream and play it.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextToAudio)]
[ExampleResourceUse(Resource.AzureAIFoundry, AIModel.TTS)]
[ExampleCostEstimate(0.01)]
public class TextToAudioExample(AzureAIFoundrySettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var project = settings.Projects.ForDeployedModel(nameof(AzureAIFoundryModelDeploymentSettings.TTS));

        var openAIClient = new AzureOpenAIClient(new Uri(project.OpenAIEndpoint), new AzureKeyCredential(project.ApiKey));
        var audioClient = openAIClient.GetAudioClient(project.DeployedModels.TTS);

        var voice = GeneratedSpeechVoice.Shimmer;
        var options = new SpeechGenerationOptions { ResponseFormat = GeneratedSpeechFormat.Mp3 };

        var text = $"""
                   This is a test audio sample using the voice of {voice.ToString()}. 
                   Peter Piper picked a peck of pickled peppers.
                   A peck of pickled peppers Peter Piper picked.
                   If Peter Piper picked a peck of pickled peppers,
                   Where's the peck of pickled peppers Peter Piper picked?
                   """;

        var result = await audioClient.GenerateSpeechAsync(text, voice , options);
        await using var audioStream = result.Value.ToStream();

        Console.WriteLine("Audio playing ...");

        await PlayAudioAsync(audioStream);

        Console.WriteLine("... complete.");
    }

    public static Task PlayAudioAsync(Stream stream)
    {
        var taskCompletionSource = new TaskCompletionSource();
        var waveOut = new WaveOutEvent();
        var mp3Reader = new Mp3FileReader(stream);
        
        waveOut.Init(mp3Reader);

        waveOut.PlaybackStopped += (_, ex) =>
        {
            waveOut.Dispose();
            mp3Reader.Dispose();

            if (ex.Exception != null)
            {
                taskCompletionSource.SetException(ex.Exception);
            }
            else
            {
                taskCompletionSource.SetResult();
            }
        };

        waveOut.Play();

        return taskCompletionSource.Task;
    }
}
