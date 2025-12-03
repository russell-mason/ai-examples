namespace OpenApiWebApi.Endpoints;

public static class PostUserEndpoint
{
    public static void RegisterPostUserEndpoint(this IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder.MapPost("/users", Execute)
                       .WithName("CreateUser")
                       .WithSummary("Creates a new user from the request body.")
                       .WithDescription("Returns the created user, minus their password, along with the retrieval URL in the Location header." +
                                        "<br>This API is for testing purposes ONLY.")
                       .Produces<CreateUserResponse>(201)
                       .Produces<ErrorResponse>(400)
                       .Produces(500);
    }

    private static IResult Execute([FromBody] CreateUserRequest request, IUserService userService)
    {
        var userResponse = userService.CreateUser(request);

        return userResponse == null
            ? Results.BadRequest(new { Error = "A user with that username already exists." })
            : Results.CreatedAtRoute("GetUser", new { userResponse.UserName }, userResponse);
    }
}
