namespace OpenApiWebApi.Models;

public class User
{
    public int Id { get; set; }

    public required string UserName { get; set; }

    public required string Password { get; set; }

    public required string Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}
