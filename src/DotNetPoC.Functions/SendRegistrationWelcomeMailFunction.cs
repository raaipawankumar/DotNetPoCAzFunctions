using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using DotNetPoC.Functions.Shared;

namespace DotNetPoC.Functions;

public class SendRegistrationWelcomeMailFunction(SendGridClient emailClient)
{
  [Function("SendRegistrationWelcomeMail")]
  public async Task RunAsync([QueueTrigger("%RegistrationWelcomeMailQueue%")] QueueMessage message)
  {
    var result = AppJsonSerializer.Deserialize<WelcomeEmailInput>(message.Body.ToString());
    result.ThrowExceptionIfFail(ex => ex ?? new Exception("Exception occurred in queue message deserialize."));

    var input = result.Value;
    var emailMessage = MailHelper.CreateSingleEmail(
      from: new EmailAddress(Environment.GetEnvironmentVariable("SENDGRID_FROM_EMAIL")),
      to: new EmailAddress(input!.Email),
      subject: "Welcome to DotNetPoC",
      plainTextContent: string.Empty,
      htmlContent: $"<strong>Welcome {input.UserName}</strong>");

    var response = await emailClient.SendEmailAsync(emailMessage);
    if (!response.IsSuccessStatusCode)
    {
      var responseMessage = await response.Body.ReadAsStringAsync();
      throw new Exception(responseMessage);
    }

  }
}


