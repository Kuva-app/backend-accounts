namespace Kuva.Accounts.Business
{
    public enum AccountsErrors
    {
        InvalidUserId,
        PasswordNullOrEmpty,
        PasswordToBig,
        ServiceBusQueueConfigurationNull,
        ServiceBusQueueConfigurationMessageToBig,
        ProcessorQueueNotInstantiate,
        UserNotFound,
        RequestChangePasswordTokenAlreadyRegistered,
        RequestChangePasswordTokenNotRegistered,
        InvalidUserEmail,
        EmailAlreadyExists,
        RequestChangePasswordTokenExpired
    }
}