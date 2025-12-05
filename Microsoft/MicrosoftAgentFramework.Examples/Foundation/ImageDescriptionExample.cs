namespace MicrosoftAgentFramework.Examples.Foundation;

// LM Studio
// https://lmstudio.ai/
// load qwen/qwen3-vl-4b

// Images taken from https://www.pixelstalk.net/

/// <summary>
/// Demonstrates how to get a description of what's shown in a set of images.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.ImageAnalysis)]
[ExampleResourceUse(Resource.LocalLMStudio, AIModel.Qwen3)]
[ExampleResourceUse(Resource.LocalFile)]
[ExampleCostEstimate(0.00)]
public class ImageDescriptionExample(LMStudioAISettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        const string instructions = """
                                    You are an image recognition AI model.
                                    Only answer the specific question asked. 
                                    Do not embellish the answer, or ask further questions.
                                    """;

        var agent = new OpenAIClient(new ApiKeyCredential("NOT_APPLICABLE"), new OpenAIClientOptions { Endpoint = new Uri(settings.Endpoint) })
                    .GetChatClient(settings.Qwen3ModelId)
                    .CreateAIAgent(instructions);

        var imagePaths = Directory.GetFiles(@".\Foundation\SourceImages\Animals", "*.jpg");

        foreach (var imagePath in imagePaths)
        {
            var data = await File.ReadAllBytesAsync(imagePath);

            var message = new ChatMessage(ChatRole.User,
            [
                new TextContent("Describe this image, including the surroundings and the object which is the focus of the image."),
                new DataContent(data, "image/jpg")
            ]);

            var response = await agent.RunAsync(message);

            Console.WriteLine($"Image File: {imagePath}");
            Console.WriteLine(response.Text);
            Console.WriteLine();
        }
    }
}
