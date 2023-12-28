namespace TMS.Notifications.Infrastructure.Emails;

public class SmptConfig
{
    public string Host { get; set; }

    public int Port { get; set; }

    public string User { get; set; }

    public string Password { get; set; }
}
