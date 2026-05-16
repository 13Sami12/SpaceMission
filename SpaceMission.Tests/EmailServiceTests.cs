using System;
using SpaceMission.Core;
using Xunit;

namespace SpaceMission.Tests;

public class EmailServiceTests
{
    [Fact]
    public void CreateMessage_PopulatesMailMessageCorrectly()
    {
        var settings = new EmailSettings(
            Host: "smtp.example.com",
            Port: 587,
            Sender: "sender@example.com",
            Recipient: "recipient@example.com",
            Username: "user",
            Password: "pass",
            EnableSsl: true);

        var message = EmailService.CreateMessage(settings, "Mission summary body");

        Assert.Equal("SpaceMission summary", message.Subject);
        Assert.Equal("Mission summary body", message.Body);
        Assert.Equal("sender@example.com", message.From.Address);
        Assert.Equal("recipient@example.com", Assert.Single(message.To).Address);
    }
}
