namespace MicrosoftSemanticKernel.Examples.Plugins.Native;

public class TaxCodeServicePlugin(ITaxCodeService taxCodeService)
{
    [KernelFunction("get_tax_code")]
    [Description("Provides a tax code for the specified company name.")]
    public string GetTaxCode(string companyName) => taxCodeService.GetTaxCode(companyName);
}
