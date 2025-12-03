namespace MicrosoftSemanticKernel.Examples.Plugins.Native.Services;

public class TaxCodeService : ITaxCodeService
{
    public string GetTaxCode(string companyName) =>
        companyName switch
        {
            "The Big Blue Box Company" => "BBX123",

            "Yellow Sky Investors" => "YSI456",

            "Purple Car Hire" => "PCH789",

            _ => "Unknown"
        };
}
