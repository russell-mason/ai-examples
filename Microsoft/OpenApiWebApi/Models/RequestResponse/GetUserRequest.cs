namespace OpenApiWebApi.Models.RequestResponse;

/// <summary>
/// Details to use when requesting an existing user.
/// </summary>
public class GetUserRequest
{
    /// <summary>
    /// The username (login) associated with the user account.
    /// </summary>
    [MaxLength(100)]
    public required string UserName { get; set; }
}
