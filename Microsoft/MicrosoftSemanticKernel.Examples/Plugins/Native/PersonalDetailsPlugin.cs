namespace MicrosoftSemanticKernel.Examples.Plugins.Native;

// Obviously all made up and only works in this example!

public class PersonalDetailsPlugin
{
    [KernelFunction("get_personal_details")]
    [Description("Provides a list of people, their date of birth, telephone number, and times when they're available to be contacted.")]
    public static Person[] GetPeople() =>
    [
        new("Mike Jones", new DateTime(1980, 2, 13), "07362917653", "6am to 4pm"),
        new("Bob Smith", new DateTime(1990, 6, 28), "01845279642", "9am to 5:30pm"),
        new("Mary Hall", new DateTime(195, 9, 21), "06574908803", "10am to 1pm")
    ];

    [KernelFunction("get_area-code")]
    [Description("Extracts the area code from a telephone number.")]
    public static string GetAreaCode(string telephoneNumber) => telephoneNumber[..5];
}
