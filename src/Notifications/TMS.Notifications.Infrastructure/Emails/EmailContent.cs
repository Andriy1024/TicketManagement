namespace TMS.Notifications.Infrastructure.Emails;

public class EmailContent
{
    public required string To { get; init; }

    public required string Subject { get; init; }

    public required string TemplatePath { get; init; }

    public object? TemplateModel { get; init; }
}