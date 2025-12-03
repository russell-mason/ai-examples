namespace OpenApiWebApi.Models.RequestResponse;

/// <summary>
/// Details to use when creating a new user.
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// The username (login) of the user to create.
    /// This must be unique. 
    /// </summary>
    [MaxLength(100)]
    public required string UserName { get; set; }

    /// <summary>
    /// The login password for the user.
    /// </summary>
    [MinLength(6)]
    [MaxLength(100)]
    public required string Password { get; set; }

    /// <summary>
    /// The email address for the user.
    /// For testing simplicity, this does not have to be unique, i.e. the same email can be used for multiple users
    /// </summary>
    [MaxLength(100)]
    public required string Email { get; set; }

    /// <summary>
    /// The user's first name.
    /// </summary>
    [MaxLength(100)]
    public string? FirstName { get; set; }

    /// <summary>
    /// The user's last name.
    /// </summary>
    [MaxLength(100)]
    public string? LastName { get; set; }

    public static User ToUser(int id, CreateUserRequest request) => new()
                                                                    {
                                                                        Id = id,
                                                                        UserName = request.UserName,
                                                                        Email = request.Email,
                                                                        Password = request.Password,
                                                                        FirstName = request.FirstName,
                                                                        LastName = request.LastName
                                                                    };
}
