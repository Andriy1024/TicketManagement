﻿using System.Reflection;

using FluentEmail.Core;

using Polly;
using Polly.CircuitBreaker;

using TMS.Common.IntegrationEvents.Notifications;
using TMS.Notifications.Application.Interfaces;

using TMS.Notifications.Domain.Models;

namespace TMS.Notifications.Infrastructure.Emails;

internal sealed class EmailsService : IEmailsService
{
    private readonly IFluentEmail _sender;

    private readonly string _viewsFolder;

    private static readonly CircuitBreakerPolicy _circuitBreaker = Policy
        .Handle<Exception>()
        .CircuitBreaker(2, TimeSpan.FromSeconds(30));

    public EmailsService(IFluentEmail sender)
    {
        _sender = sender;

        _viewsFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            ?? throw new InvalidOperationException("Failed to get app folder");

        _viewsFolder = Path.Combine("Views");
    }

    public async Task SendAsync(NotificationEntity entity)
    {
        EmailContent email = entity.Payload switch
        {
            OrderStatusUpdatedNotification orderUpdated 
                => CreateEmail(orderUpdated),
            
            _ => throw new InvalidOperationException("Unknown notification type")
        };

        await _circuitBreaker
            .Wrap(Policy
                .Handle<Exception>()
                .WaitAndRetry(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
            .Execute(() => _sender
                .To(email.To)
                .Subject(email.Subject)
                .UsingTemplateFromFile(email.TemplatePath, email.TemplateModel)
                .SendAsync()
            );
    }

    private EmailContent CreateEmail(OrderStatusUpdatedNotification orderUpdated)
    {
        return new EmailContent 
        {
            To = GetToEmail(orderUpdated.AccountId),
            Subject = "TMS: Order Updated",
            TemplatePath = Path.Combine(_viewsFolder, "OrderUpdated.cshtml"),
            TemplateModel = orderUpdated
        };
    }

    private string GetToEmail(long accountId)
    {
        // TODO: get user email from Identity Servicer API

        return "test@fake.com";
    }   
}