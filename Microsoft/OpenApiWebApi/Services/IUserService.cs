namespace OpenApiWebApi.Services;

public interface IUserService
{
    public GetUserResponse? GetUser(GetUserRequest request);

    public CreateUserResponse? CreateUser(CreateUserRequest request);
}
