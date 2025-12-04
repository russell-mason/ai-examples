# Getting Started

These examples can be executed via the **MicrosoftAgentFramework.ConsoleApp** project.

### Intent

These examples are for learning purposes, and intended to show the features provided by the Agent Framework. 

Many examples mirror those from the Semantic Kernel project, simply because the Agent Framework is its successor and 
illustrating the similarities and differences is a useful exercise.

Examples attempt to be self-contained. While this results in duplication of code, the idea is to be able to go to an 
example and just see the code required to complete that task. There are some exceptions to this, such as injecting 
services to provide broader and more realistic scenarios, but these should be fairly self-evident from the context.

All cost estimates are in USD. The minimum estimates is $0.001, i.e. a tenth of a cent. Some examples only use a few 
tokens so would be less, but for the sake of simplicity a tenth of a cent seems reasonable. Estimates are purely an 
indicator of scale, i.e. a fraction of a cent, a few cents, etc. They are not intended to be accurate based on actual token 
usage.

**N.B.** Some examples here can only be used via an Azure account using Azure Credentials, as opposed to an API key which 
most other examples use. These are clearly highlighted, and you'll need to be logged in to your Azure account for them
to work.  
Simply using ***"az login"*** via the Azure CLI is sufficient.  
https://learn.microsoft.com/en-us/cli/azure/authenticate-azure-cli-interactively?view=azure-cli-latest

# Examples

### Foundation

- **ChatClientExample**  
Demonstrates creating the individual elements required to obtain a chat client, and getting a simple response from a prompt. 
No agent involved.  
Model: gpt-4o-mini

- **AgentFromExistingChatClientExample**  
Demonstrates creating the individual elements required to obtain an agent, and getting a simple response from a prompt.  
Model: gpt-4o-mini

- **MultipleAgentsFromExistingChatClientExample**  
Demonstrates creating two independent agents using a common chat client.  
Model: gpt-4o-mini

- **AgentChatClientExample**  
Demonstrates simplifying the creation of an agent rather than using the individual elements explicitly.  
Model: gpt-4o-mini

- **AgentChatClientStreamingExample**  
Demonstrates getting a streamed response from an agent, and  outputting response tokens as they're received.  
Model: gpt-4o-mini

- **AgentChatClientOptionsExample**  
Demonstrates providing options to the agent, such as a starting prompt and temperature.  
Model: gpt-4o-mini

- **AgentChatClientTokenUsageExample**  
Demonstrates how to get the number of input and output tokens used by agent request/response messages.  
Model: gpt-4o-mini

- **AgentChatClientThreadExample**  
Demonstrates how to create a thread (chat history) so that prior request/response messages provide historical context.  
Model: gpt-4o-mini

- **MessageCountingChatReducerExample**  
Demonstrates using a chat reducer with an agent to only include a set number of request/response messages. 
This will result in older messages being removed so context from further back will no longer be available.    
Model: gpt-4o-mini

- **SummarizingChatReducerExample**  
Demonstrates using a chat reducer with an agent to summarize messages thus far. This will create a single 
message from a set of messages and replace them. This should provide enough context without having to still have 
all the literal messages available.    
Model: gpt-4o-mini

- **AzureModelExample**  
Demonstrates using an Azure hosted grok-3-mini model with an agent.  
Model: grok-3-mini

- **StructuredDataExample**  
Demonstrates how to return a typed response message that contains structured data directly.  
Model: gpt-4o-mini

- **StructuredDataFromJsonSchemaExample**  
Demonstrates how to return a message that contains JSON conforming to provided JSON schema.  
Model: gpt-4o-mini

- **TranscribeAudioExample**  
Demonstrates how to read an mps3 audio file and transcribe it to text using an audio client.  
Model: gpt-4o-mini

- **LocalModelExample**  
Demonstrates using the LM Studio application running locally with a microsoft/phi-4 model.    
Model: microsoft/phi-4

- **DataContentExample**  
Demonstrates how to include image content in a chat message, and provide a description of what's shown in the
image.
Model: qwen/qwen3-vl-4b

- **ImageDescriptionExample**  
Demonstrates how to get a description of what's shown in a set of images.  
Model: qwen/qwen3-vl-4b

- **VehicleDamageAssessmentExample**  
Demonstrates image analysis, determining whether a set of vehicles exhibit any damage and, if so, what type of damage,
and where it appears.  
Model: qwen/qwen3-vl-4b

### Tools

- **FunctionToolsViaOptionsExample**  
Demonstrates how to add tools from static functions via options that can be passed to an agent.   
Model: gpt-4o-mini

- **FunctionToolsViaCreationExample**  
Demonstrates how to create tools from static functions and include them when creating the agent.  
Model: gpt-4o-mini

- **FunctionToolsViaSelfExposedExample**  
Demonstrates how a class can expose a series of functions as AI Tools.  
Model: gpt-4o-mini

- **ToolWithDependencyInjectionExample**  
Demonstrates how a service collection can be passed to the agent so that tools can have services injected into them.  
Model: gpt-4o-mini

- **FunctionToolApprovalExample**  
Demonstrates requiring the user to approve the calling of a tool from an agent.  
Model: gpt-4o-mini

- **AgentAsToolExample**  
Demonstrates how to use an agent as a tool from within another agent.  
Model: gpt-4o-mini

- **OpenApiToolExample**  
Demonstrates using a persistent agent with an OpenAPI schema tool that infers a set of functions that can then be used 
automatically given the context of a prompt.  
Model: gpt-4o-mini  
**N.B.** This uses a PersistentAgent which is only available via an Azure account using Azure Credentials. You will 
need to be logged in to your Azure account to run this example.  
**N.B.** The associated API must also have been deployed and publicly available (see the OpenApiWebApi project).
