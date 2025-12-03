namespace OpenApiWebApi.Endpoints;

public static class GetUserEndpoint
{
    public static void RegisterGetUserEndpoint(this IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder.MapGet("/users/{UserName}", Execute)
                       .WithName("GetUser")
                       .WithSummary("Gets a user with the specified user name (login).")
                       .WithDescription("Returns the user if they exist, otherwise not found." +
                                        "<br>This API is for testing purposes ONLY.")
                       .Produces<GetUserResponse>()
                       .Produces(404);
    }

    private static IResult Execute([AsParameters] GetUserRequest request, IUserService userService)
    {
        var userResponse = userService.GetUser(request);

        return userResponse == null ? Results.NotFound() : Results.Ok(userResponse);
    }
}
