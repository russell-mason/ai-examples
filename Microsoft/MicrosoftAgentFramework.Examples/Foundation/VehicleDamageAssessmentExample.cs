namespace MicrosoftAgentFramework.Examples.Foundation;

// LM Studio
// https://lmstudio.ai/
// load qwen/qwen3-vl-4b

public class VehicleDamageAssessmentExample(LMStudioAISettings settings) : IExample
{
    public async Task ExecuteAsync()
    {
        var agentOptions = new ChatClientAgentOptions
                           {
                               Instructions = """
                                              You are a vehicle damage assessment AI model.
                                              Only answer the specific question asked. 
                                              Do not embellish the answer, or ask further questions.
                                              The image should be analyzed to determine if there is any visible damage to the vehicle.
                                              Use UK english language, e.g. windscreen, Assume UK vehicle models, i.e. The Driver's side is on the right.
                                              IsDamaged: True if the vehicle is damaged, false if not
                                              AreaOfDamage: If there is damage, provide the specific area of the vehicle that is damaged.
                                                            If there is no damage, select None.
                                              Description: Provide a description of the damage up to two sentences. 
                                                           If there is no damage, leave this field empty
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

        var imagePaths = Directory.GetFiles(@".\Foundation\SourceImages\Vehicles", "*.jpg");

        foreach (var imagePath in imagePaths)
        {
            var data = await File.ReadAllBytesAsync(imagePath);

            var message = new ChatMessage(ChatRole.User, [new DataContent(data, "image/jpg")]);

            var response = await agent.RunAsync<DamageAssessment>(message);

            Console.WriteLine($"Image File: {imagePath}");
            Console.WriteLine($"Damaged: {response.Result.IsDamaged}");

            if (response.Result.IsDamaged)
            {
                Console.WriteLine($"Area: {response.Result.AreaOfDamage}");
                Console.WriteLine($"Damage: {response.Result.Description}");
            }

            Console.WriteLine();
        }
    }
}
