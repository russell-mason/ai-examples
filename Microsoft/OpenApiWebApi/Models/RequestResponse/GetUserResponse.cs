namespace OpenApiWebApi.Models.RequestResponse;

/// <summary>
/// The user, including login and profile details.
/// </summary>
public class GetUserResponse
{
    /// <summary>
    /// The unique ID for the user.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// The username, also known as a login.
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    /// The email address of the user.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The user's first name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// The user's last name.
    /// </summary>
    public string? LastName { get; set; }

    public static GetUserResponse FromUser(User user) => new()
                                                         {
                                                             Id = user.Id,
                                                             UserName = user.UserName,
                                                             Email = user.Email,
                                                             FirstName = user.FirstName,
                                                             LastName = user.LastName
                                                         };
}
