namespace MicrosoftAgentFramework.Examples.Foundation;

// LM Studio
// https://lmstudio.ai/
// load qwen/qwen3-vl-4b

/// <summary>
/// Demonstrates how to extract text from an image.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.ImageAnalysis)]
[ExampleResourceUse(Resource.LocalLMStudio, AIModel.Qwen3)]
[ExampleResourceUse(Resource.LocalFile)]
[ExampleCostEstimate(0.00)]
public class HandWritingExample(LMStudioAISettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var agentOptions = new ChatClientAgentOptions
                           {
                               Instructions = """
                                              You are a hand writing image recognition AI model.
                                              Only provide a literal interpretation of the text in the image, 
                                              keeping case, punctuation, symbols, and line breaks. 
                                              Ignore anything that is not text. 
                                              """,
                               ChatOptions = new ChatOptions
                                             {
                                                 Temperature = 0,
                                                 TopP = 0
                                             }
                           };

        var agent = new OpenAIClient(new ApiKeyCredential("NOT_APPLICABLE"), new OpenAIClientOptions { Endpoint = new Uri(settings.Endpoint) })
                    .GetChatClient(settings.Qwen3ModelId)
                    .CreateAIAgent(agentOptions);

        var imagePaths = Directory.GetFiles(@".\Foundation\SourceImages\Handwriting", "*.jpg");

        foreach (var imagePath in imagePaths)
        {
            var data = await File.ReadAllBytesAsync(imagePath);

            var message = new ChatMessage(ChatRole.User,
            [
                new DataContent(data, "image/jpg")
            ]);

            var response = await agent.RunAsync(message);

            Console.WriteTitle($"Image File: {imagePath}");
            Console.WriteLine(response.Text);
            Console.WriteLine();
        }
    }
}
