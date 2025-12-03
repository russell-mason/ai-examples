namespace MicrosoftSemanticKernel.Examples.Plugins.Native.Services;

public interface ITaxCodeService
{
    string GetTaxCode(string companyName);
}
