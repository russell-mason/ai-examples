namespace MicrosoftAgentFramework.Examples.Tools;

public class TaxCodeFunctions(ITaxCodeService taxCodeService)
{
    [Description("Provides a tax code for the specified company name.")]
    public string GetTaxCode(string companyName) => taxCodeService.GetTaxCode(companyName);

    public IList<AITool> AsAITools() => new List<AITool> { AIFunctionFactory.Create(GetTaxCode) };
}
