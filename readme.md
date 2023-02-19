
# USA Visa Checker

This is a .NET Core Azure Function that periodically checks for available dates from the USA Visa application website. It is designed to send an email notification to a recipient when a new available date is found.

## Configuration

The function requires the following configurations:

### Email Options

The email options configuration should include the sender's email address and default display name. Update the `FromAddress` and `DefaultFromDisplayName` values in `local.settings.json` file.

`"EmailOptions": {
    "FromAddress": "your_email@example.com",
    "DefaultFromDisplayName": "Your Name"
}` 

### Email Provider Password

The email provider password configuration should include the password for the email provider you are using. Update the `EmailProviderPassword` value in `local.settings.json` file.



`"EmailProviderPassword": "your_email_provider_password"` 

### Visa Account Details

The visa configuration should include the email and password for your USA Visa application account. Update the `Email` and `Pass` values in `local.settings.json` file.


`"Visa": {
    "Email": "your_email@example.com",
    "Pass": "your_password"
}` 

### Initial Date

The initial date configuration should include the date from which you want the function to start searching for available dates. Update the `InitialDate` value in `local.settings.json` file.


`"InitialDate": "2023-01-01"` 

### Email Send

The email send configuration should include the recipient's email address. Update the `Recipient` value in `local.settings.json` file.


`"EmailSend": {
    "Recipient": "recipient_email@example.com"
}` 

### Application ID

The application ID configuration should include the application ID for your visa application. Update the `ApplicationId` value in `local.settings.json` file.

`"ApplicationId": "your_application_id"` 

## Deployment

To deploy this function to Azure, you can follow the [official documentation](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-first-azure-function-azure-cli).

## Config File Content
```json
{
"EmailOptions": {
    "FromAddress": "",
    "DefaultFromDisplayName": ""
  },
  "EmailProviderPassword": "",
  "Visa": {
    "Email": "",
    "Pass": ""
  },
  "InitialDate": "2023-01-01",
  "EmailSend": {
    "Recipient": ""
  },
  "ApplicationId": ""
}
```

## Usage

Once the function is deployed and configured, it will periodically check for available dates on the USA Visa application website. If a new date is found, an email notification will be sent to the recipient's email address specified in the configuration.
