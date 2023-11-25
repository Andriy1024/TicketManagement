namespace TMS.Common.Users;

public sealed record CurrentUser(int Id, string Email);

public interface IUserContext 
{
    CurrentUser GetUser();
}

public sealed class UserContext : IUserContext
{
    public CurrentUser GetUser()
    {
        return new(12, "user@fake.com");
    }
}