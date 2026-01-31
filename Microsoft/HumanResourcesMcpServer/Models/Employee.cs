namespace HumanResourcesMcpServer.Models;

public record Employee(string EmployeeNumber, 
                       string Name, 
                       DateTime DateOfBirth, 
                       string TelephoneNumber, 
                       DateTime DateJoined,
                       decimal AnnualSalary,
                       string TimesAvailable);
