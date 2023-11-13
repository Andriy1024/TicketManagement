using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.Users;

public sealed class CurrentUser
{
    public int Id { get; init; }

    public string Email { get; init; }
}

public interface IUserContext 
{
    CurrentUser GetUser();
}

public sealed class UserContext : IUserContext
{
    public CurrentUser GetUser()
    {
        return new CurrentUser() 
        {
            Id = 12,
            Email = "user@fake.com"
        };
    }
}