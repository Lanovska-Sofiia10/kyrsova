using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Kyrsova.Messages;

public class NotificationService : INotificationService
{
    private readonly string accountSid = "your_account_sid";
    private readonly string authToken = "your_auth_token";
    private readonly string twilioPhoneNumber = "+1234567890";
    public NotificationService()
    {
        TwilioClient.Init(accountSid, authToken);
    }

    public void SendConfirmationCode(string cellPhone, int code)
    {
        string messageBody = $"Ваш код підтвердження: {code}";

        try
        {
            var message = MessageResource.Create(
                body: messageBody,
                from: new PhoneNumber(twilioPhoneNumber),
                to: new PhoneNumber(cellPhone)
            );

            Console.WriteLine($"Повідомлення з кодом {code} надіслано на номер: {cellPhone}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при відправці повідомлення: {ex.Message}");
        }
    }
}