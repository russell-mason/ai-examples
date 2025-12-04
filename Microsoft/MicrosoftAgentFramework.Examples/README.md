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
