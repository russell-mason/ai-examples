namespace OpenApiWebApi.Models.RequestResponse;

/// <summary>
/// Details to use when requesting whether a user exists or not.
/// </summary>
public class GetUserExistsRequest
{
    /// <summary>
    /// The username (login) associated with the user account to check for.
    /// </summary>
    [MaxLength(100)]
    public required string UserName { get; set; }
}
