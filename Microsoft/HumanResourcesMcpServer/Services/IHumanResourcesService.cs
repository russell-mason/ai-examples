namespace HumanResourcesMcpServer.Services;

public interface IHumanResourcesService
{
    Employee[] GetEmployees();

    Employee[] GetEmployees(int top, string sortPropertyName, ListSortDirection sortDirection);

    Employee[] GetEmployees(string name);

    string GetAreaCode(string telephoneNumber);
}
