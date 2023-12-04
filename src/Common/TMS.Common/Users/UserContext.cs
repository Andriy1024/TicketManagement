namespace TMS.Common.Users;

public sealed record CurrentUser(int Id, string Email);

public interface IUserContext 
{
    CurrentUser GetUser();
}

public sealed class UserContext : IUserContext
{
    public static readonly int DefaultId = 12;

    public CurrentUser GetUser()
    {
        return new(DefaultId, "user@fake.com");
    }
}