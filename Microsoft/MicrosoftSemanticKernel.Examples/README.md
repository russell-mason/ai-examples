# Getting Started

These examples can be executed via the **MicrosoftSemanticKernel.ConsoleApp** project.

### Intent

These examples are for learing purposes, and intended to show the features provided by Semantic Kernel. 

Examples attempt to be self-contained. While this results in duplication of code, the idea is to be able to go to an 
example and just see the code required to complete that task. There are some exceptions to this, such as injecting 
services to provide broader and more realistic scenarios, but these should be fairly self-evident from the context.

All cost estimnates are in USD. The minimum estimates is $0.001, i.e. a tenth of a cent. Some examples only use a few 
tokens so would be less, but for the sake of simplicity a tenth of a cent seems reasonable. Estimates are purely an 
indicator of scale, i.e. a fraction of a cent, a few cents, etc. They are not intended to be accurate based on actual token 
usage.

# Examples

### Foundation

- **ChatCompletionExample**  
Demonstrates obtaining a chat completion service from the kernel, and getting a simple response from a prompt.  
Model: gpt-4o-mini

- **ChatCompletionStreamingExample**  
Demonstrates getting a streamed response from a chat completion service, and outputting response tokens as they're received.  
Model: gpt-4o-mini

- **ChatCompletionSettingsExample**  
Demonstrates providing options to the chat completion service, such as a starting prompt and temperature.  
Model: gpt-4o-mini

- **ChatCompletionTokenUsageExample**  
Demonstrates how to get the number of input and output tokens used by chat request/response messages.  
Model: gpt-4o-mini

- **ChatCompletionHistoryExample**  
Demonstrates how to create a chat history so that prior request/response messages provide historical context.  
Model: gpt-4o-mini

- **ChatCompletionHistoryTruncationReducerExample**  
Demonstrates how to take a chat history and condense it to only include a set number of request/response messages. 
This will result in older messages being removed so context from further back will no longer be available.  
Model: gpt-4o-mini

- **ChatCompletionHistorySummarizationReducerExample**  
Demonstrates how to take a chat history and condense it to a summary of the messages thus far. This will create 
a single message from a set of messages and replace them. This should provide enough context without having to 
still have all the literal messages available.  
Model: gpt-4o-mini

### Model Providers

- **AzureOpenAIExample**  
Demonstrates using a gpt-4o-mini model via Azure and the OpenAI chat client.  
Model: gpt-4o-mini

- **AzureGrokExample**  
Demonstrates using a grok-3-mini model via Azure and the OpenAI chat client.  
Model: grok-3-mini

- **GitHubAIInferenceExample**  
Demonstrates using a meta/Meta-Llama-3.1-405B-Instruct model via GitHub and the Azure AI Inference chat client.  
Model: meta/Meta-Llama-3.1-405B-Instruct

- **OllamaLocalPhiExample**  
Demonstrates using the Ollama application running locally with a phi3:mini model via the Ollama chat client.  
Model: phi3:mini

- **OllamaStandaloneLocalPhiExample**  
Demonstrates using the Ollama application running locally with a phi3:mini model via a standalone instance of the Ollama 
chat client, i.e. without using the kernal.  
Model: phi3:mini

- **LMStudioLocalPhiExample**  
Demonstrates using the LM Studio application running locally with a microsoft/phi-4 model via the OpenAI chat client.  
Model: microsoft/phi-4

### Multimodal Models

- **ImageContentExample**  
Demonstrates how to use the OpenAI chat client to analyse whether there are any animals in an image, and if so, what they are.  
Model: qwen/qwen3-vl-4b

- **ImageDescriptionExample**  
Demonstrates how to use the OpenAI chat client to describe what's shown in an image.  
Model: qwen/qwen3-vl-4b

### Audio To Text

- **AudioToTextExample**  
Demonstrates how to read an mps3 audio file and transcribe it to text using the Azure Open AI audio to text service.  
Model: whisper
