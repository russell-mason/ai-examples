namespace OpenApiWebApi.Models;

/// <summary>
/// Error message details.
/// </summary>
/// <param name="message">The error message.</param>
public class ErrorResponse(string message)
{

    /// <summary>
    /// A message containing details of the error that occurred.
    /// </summary>
    public string Message => message;
}
