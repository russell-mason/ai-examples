namespace HumanResourcesMcpServer.Tools;

[McpServerToolType]
public class HumanResourcesTools(IHumanResourcesService humanResourcesService)
{
    [McpServerTool]
    [Description("Returns an unordered list of all employees with no filtering. " +
                 "Intended for Human Resources (HR) scenarios. " +
                 "Each employee record includes: EmployeeNumber, Name, DateOfBirth, TelephoneNumber, DateJoined, AnnualSalary, and TimesAvailable.")]
    public Employee[] GetEmployees() => humanResourcesService.GetEmployees();

    [McpServerTool]
    [Description("Returns a list of employees that can be sorted in either ascending or descending order. " +
                 "Intended for Human Resources (HR) scenarios. " +
                 "Each employee record includes: EmployeeNumber, Name, DateOfBirth, TelephoneNumber, DateJoined, AnnualSalary, and TimesAvailable.")]
    public Employee[] GetEmployeesSortedBy([Description("Limits the number of records returned. Defaults to 10.")] int top = 10,
                                           [Description("The property to sort by. " +
                                                        "Can use EmployeeNumber, Name, DateOfBirth, TelephoneNumber, DateJoined, AnnualSalary, and TimesAvailable." +
                                                        "Defaults to 'Name'.")]
                                           string sortPropertyName = nameof(Employee.Name),
                                           [Description("Whether the sorting is Ascending or Descending. Defaults to Ascending.")]
                                           ListSortDirection sortDirection = ListSortDirection.Ascending) =>
        humanResourcesService.GetEmployees(top, sortPropertyName, sortDirection);

    [McpServerTool]
    [Description("Returns a list of employees with the specified name (case insensitive). N.B. Multiple employees may have the same name. " +
                 "Intended for Human Resources (HR) scenarios. " +
                 "Each employee record includes: EmployeeNumber, Name, DateOfBirth, TelephoneNumber, DateJoined, AnnualSalary, and TimesAvailable.")]
    public Employee[] GetEmployeesByName([Description("The name to filter by.")] string name) => humanResourcesService.GetEmployees(name);

    [McpServerTool]
    [Description("Extracts the area code from a telephone number.")]
    public static string GetAreaCode(string telephoneNumber) => telephoneNumber[..5];
}
