namespace OpenApiWebApi.Services;

// This is explicitly designed to demonstrate the prompt from OpenApi examples.
// i.e. There is no multi-threading, no validation, no error handling, etc.

public class UserService : IUserService
{
    private DateTime _lastClearedAt = DateTime.UtcNow;
    private readonly List<User> _users = [];

    public GetUserResponse? GetUser(GetUserRequest request)
    {
        var user = _users.FirstOrDefault(user => user.UserName == request.UserName);

        return (user is null) ? null : GetUserResponse.FromUser(user);
    }

    public CreateUserResponse? CreateUser(CreateUserRequest request)
    {
        var existingUser = _users.FirstOrDefault(user => user.UserName == request.UserName);

        if (existingUser is not null) return null; // Already exists

        ManageList();

        var nextId = _users.Count == 0 ? 1 : _users.Max(user => user.Id) + 1;
        var user = CreateUserRequest.ToUser(nextId, request);

        _users.Add(user);

        return CreateUserResponse.FromUser(user);
    }

    public void ManageList()
    {
        // Make sure the list is manageable by allowing only 50 items for 24 hours

        if (_lastClearedAt < DateTime.UtcNow.AddDays(-1))
        {
            _users.Clear();

            _lastClearedAt = DateTime.UtcNow;
        }

        if (_users.Count > 50)
        {
            _users.RemoveAt(0);
        }
    }
}
