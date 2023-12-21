namespace TMS.Notifications.Infrastructure.Emails;

public class EmailConfig
{
    public bool Enabled { get; set; }

    public string FromEmail { get; set; }

    public string FromName { get; set; }

    public string? Provider { get; set; }

    public SendGridConfig? SendGrid { get; set; }

    public SmptConfig? Smpt { get; set; }
}
