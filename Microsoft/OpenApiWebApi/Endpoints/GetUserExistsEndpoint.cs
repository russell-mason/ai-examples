namespace OpenApiWebApi.Endpoints;

public static class GetUserExistsEndpoint
{
    public static void RegisterGetUserExistsEndpoint(this IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder.MapGet("/users/{UserName}/exists", Execute)
                       .WithName("GetUserExists")
                       .WithSummary("Gets whether a user with the specified user name (login) exists or not.")
                       .WithDescription("Returns true if does, false if not." +
                                        "<br>This API is for testing purposes ONLY.")
                       .Produces<GetUserResponse>()
                       .Produces<GetUserExistsResponse>();
    }

    private static IResult Execute([AsParameters] GetUserExistsRequest request, IUserService userService)
    {
        var userResponse = userService.GetUser(new GetUserRequest() { UserName = request.UserName });

        return Results.Ok(new GetUserExistsResponse() { Exists = userResponse != null });
    }
}
