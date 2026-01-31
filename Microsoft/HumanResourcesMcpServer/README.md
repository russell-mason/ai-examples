# Getting Started

The following assumes you're using Visual Studio, so adjust accordingly for other IDEs.

**N.B.** This is not intended to be a serious MCP Server, it's only intended to demonstrate how to expose 
MCP tools, and using them from within the context of Microsoft Agent Framework.

The example can be executed via the **MicrosoftAgentFramework.ConsoleApp** project. Note that the example 
automatically starts the MCP server, so doesn't need to be running separately.

### Using from within Visual Stidio Copilot chat

Outside of the example, you can experiment with this MCP server from within Visual Studio.

In the **MCPServers** solution folder, you can find a **.mcp.json** file, which configures the MCP Server. 

* From the Github Copilot tab, click the 'Select tools' icon
* Select the HumanResourcesMcpServer MCP Server, and the tools underneath it
* You can also configure whether the tools are automatically invoked, or you're prompted before use, by 
  clicking on the three dots next to the MCP server name
* Alternatively, you can open the .mcp.json file and click the options just above the MCP Server name

Onse started, you can ask Copilot Chat a question relating to the available service, for example: 
`Using HR records, does Bob Smith Earn more than Mike Jones?`. Depending on your configuration, you may be asked for 
permisstion to invoke the appropriate tool.

### Build restrictions

* This is only intended to be used from within Visual Studio on Windows, as part of this solution
* If you want to build for other platforms, you can modify the project file to explicitly target them
* The project does not configure any NuGet package details

---

Return to the repository [README](../../) file
