namespace HumanResourcesMcpServer.Services;

/// <summary>
/// N.B. This is not intended to be a real service, and is only intended to provide functions that can demonstrate
/// the selection of different methods based on different parameters.
/// </summary>
public class HumanResourcesService : IHumanResourcesService
{
    public Employee[] GetEmployees() =>
    [
        new("HR-001", "Mike Jones", new DateTime(1982, 2, 13), "07362917653", new DateTime(2020, 3, 17), 40000, "6am to 4pm"),
        new("C-012", "Bob Smith", new DateTime(1975, 6, 28), "01845279642", new DateTime(2017, 1, 1), 75000, "9am to 5:30pm"),
        new("IT-003", "Mary Hall", new DateTime(1995, 9, 10), "06574908803", new DateTime(2020, 4, 1), 55500, "10am to 1pm"),
        new("IT-008", "Terry Morgan", new DateTime(1997, 4, 2), "05296577410", new DateTime(2018, 8, 20), 48500, "9am to 9pm"),
        new("C-014", "Brian Bell", new DateTime(1972, 11, 27), "03540114921", new DateTime(2021, 11, 12), 80000, "7am to 7pm")
    ];

    public Employee[] GetEmployees(int top, string sortPropertyName, ListSortDirection sortDirection)
    {
        const BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        var employees = GetEmployees();

        var sortProperty = typeof(Employee).GetProperty(sortPropertyName, bindingFlags);

        if (sortProperty is null) throw new ArgumentException($"Property '{sortPropertyName}' not found on Employee.");

        IEnumerable<Employee> sortedEmployees = sortDirection == ListSortDirection.Ascending
            ? employees.OrderBy(employee => sortProperty.GetValue(employee, null))
            : employees.OrderByDescending(employee => sortProperty.GetValue(employee, null));

        var topEmployees = sortedEmployees.Take(top).ToArray();

        return topEmployees;
    }

    public Employee[] GetEmployees(string name) =>
        GetEmployees().Where(employee => employee.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)).ToArray();

    public string GetAreaCode(string telephoneNumber) => telephoneNumber[..5];
}
