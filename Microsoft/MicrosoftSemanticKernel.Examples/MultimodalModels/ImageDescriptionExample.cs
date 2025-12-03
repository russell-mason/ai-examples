namespace MicrosoftSemanticKernel.Examples.MultimodalModels;

// LM Studio
// https://lmstudio.ai/
// load qwen/qwen3-vl-4b
// Images taken from https://www.pixelstalk.net/

/// <summary>
/// Demonstrates how to use the OpenAI chat client to describe what's shown in an image.
/// </summary>
[ExampleCategory(Category.GettingStarted)]
[ExampleCategory(Category.TextGeneration)]
[ExampleCategory(Category.ImageAnalysis)]
[ExampleResourceUse(Resource.LocalLMStudio, AIModel.Qwen3)]
[ExampleResourceUse(Resource.LocalFile)]
[ExampleCostEstimate(0.00)]
public class ImageDescriptionExample(LMStudioAISettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var kernel = Kernel.CreateBuilder()
                           .AddOpenAIChatCompletion(settings.Qwen3ModelId, new Uri(settings.Endpoint), null)
                           .Build();

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        var imagePaths = Directory.GetFiles(@".\MultimodalModels\SourceImages\Animals", "*.jpg");

        foreach (var imagePath in imagePaths)
        {
            var imageContent = await File.ReadAllBytesAsync(imagePath);

            var chatHistory = new ChatHistory();

            chatHistory.AddSystemMessage("""
                                         You are an image recognition AI model.
                                         Only answer the specific question asked. 
                                         Do not embellish the answer, or ask further questions.
                                         """);

            chatHistory.AddUserMessage(
            [
                new ImageContent(imageContent, "image/jpg"),
                new TextContent("Describe this image, including the surroundings and the object which is the focus of the image.")
            ]);

            var response = await chatCompletionService.GetChatMessageContentAsync(chatHistory);

            Console.WriteLine($"Image File: {imagePath}");
            Console.WriteLine(response.Content);
            Console.WriteLine();
        }
    }
}
