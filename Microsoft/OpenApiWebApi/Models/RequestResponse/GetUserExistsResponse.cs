namespace OpenApiWebApi.Models.RequestResponse;

/// <summary>
/// Whether the user exists or not.
/// </summary>
public class GetUserExistsResponse
{
    /// <summary>
    /// True if the user exists, false if they don't.
    /// </summary>
    public required bool Exists { get; set; }
}
