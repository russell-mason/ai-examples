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

### Structured Data

- **JsonFromChatCompletionExample**  
Demonstrates how to return a message that contains JSON by describing the format in the prompt.  
Model: gpt-4o-mini

- **ResponseFormatFromInvokePromptExample**  
Demonstrates how to return a message that contains JSON by specifying the response format via settings, and then 
deserializing the JSON to a typed object.  
Model: gpt-4o-mini

### Prompt Functions

- **PromptToFunctionExample**  
Demonstrates how to turn a parameterized prompt into a kernel function, then execute it passing the parameter.  
Model: gpt-4o-mini

- **JsonFileToFunctionFunctionExample**  
Demonstrates how to import a plugin containing a prompt function via config.json and skprompt.txt files, and then executing 
the function from within that plugin.  
Model: gpt-4o-mini

- **YamlFileToFunctionFunctionExample**  
Demonstrates how to create a prompt function from a yaml file, then execute it passing a parameter.   
Model: gpt-4o-mini

- **TranslationFunctionExample**  
Demonstrates how to import a plugin containing a prompt function via config.json and skprompt.txt files, and executing 
it multiple times using different parameters.    
Model: gpt-4o-mini

- **FunctionPipelineExample**  
Demonstrates how to compose, and execute, multiple prompt functions via a pipeline passing a value from one function to 
the next. This uses a custom pipeline class originating from Microsoft examples.  
Model: gpt-4o-mini

### Plugins

- **NoPluginExample**  
Demonstrates how, without additional support, chat completion doesn't know basic information such as the current date.   
Model: gpt-4o-mini

- **TimePluginExample**  
Demonstrates how TimePlugin, a pre-built Microsoft plugin, provides chat completion with access to date and time related 
functionality.  
Model: gpt-4o-mini

- **TimePluginUsageExample**  
Demonstrates how to get the number of input and output tokens used by chat completion when using a plugin. This increases the 
number of tokens used as additional request and response messages are created between the chat completion client and the 
plugin.  
Model: gpt-4o-mini

- **FileIOPluginUsageExample**  
Demonstrates how FileIOPlugin, a pre-built Microsoft plugin, provides the ability to read the contents of a local file and use
that as an additional source, from within a prompt.  
Model: gpt-4o-mini

- **TextPluginUsageExample**  
Demonstrates how TextPlugin, a pre-built Microsoft plugin, provides access to text based functionality, such as converting 
case, and determining the length of text.  
Model: gpt-4o-mini

- **HttpPluginUsageExample**  
Demonstrates how HttpPlugin, a pre-built Microsoft plugin, can be used to retrieve some JSON using an HTTP call.  
Model: gpt-4o-mini

- **ConversationSummaryPluginExample**  
Demonstrates how ConversationSummaryPlugin, a pre-built Microsoft plugin, can take a block of text and create a summary from it.  
Model: gpt-4o-mini

- **PersonalDetailsPluginExample**  
Demonstrates how to create a custom plugin that provides some data that can then be included in prompt queries.  
Model: gpt-4o-mini

- **TaxCodeServicePluginExample**  
Demonstrates how to create a custom plugin that uses dependency injection to access external functionality via services.  
Model: gpt-4o-mini

### Embeddings

- **EmbeddingGeneratorExample**  
Demonstrates how to create an embedding from text.  
Model: text-embedding-3-small

- **InMemoryVectorSearchExample**  
Demonstrates populating an in-memory vector store with a set of objects with embeddings, and performing a search using a 
vector based query.   
Model: text-embedding-3-small

- **InMemoryTextSearchExample**  
Demonstrates populating an in-memory vector store with a set of objects with embeddings, and performing a search using text.  
Model: text-embedding-3-small
