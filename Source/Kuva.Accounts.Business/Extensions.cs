using System.Collections.Generic;

namespace Kuva.Accounts.Business
{
    public static class Extensions
    {
        private const string CodePrefix = "AC";
        private const string CodeFormat = "{0}{1:0####}";

        private const string DefaultMessage = "Ocorreu um erro, tente novamente mais tarde.";
        private const string InvalidUserMessage = "O Id do usuário deverá ser maior que 0.";
        private const string PasswordNullOrEmptyMessage = "O novo password não poderá ser vazio ou nulo.";
        private const string PasswordToBigMessage = "Senha muito grande.";
        private const string ServiceBusQueueConfigurationNulLMessage =
            "Configuração do Azure Service Bus não está presente.";
        private const string ServiceBusQueueConfigurationMessageToBigMessage =
            "Message {0} - {1} is too large and cannot be sent.";
        private const string ProcessorQueueNotInstantiateMessage = "Processor não foi instanciado.";
        private const string UserNotFoundMessage = "Usuário não encontrado.";
        private const string RequestChangePasswordTokenAlreadyRegisteredMessage = "Já existe um token registrado para o usuário.";
        private const string RequestChangePasswordTokenNotRegisteredMessage = "O Token não pode ser registrado.";
        private const string SearchUserEmailInvalidMessage = "E-mail inválido";
        private const string EmailAlreadyExistsMessage = "E-mail já está em uso";
        private const string RequestChangePasswordTokenExpiredMessage = "O Token está expirado";


        public static KeyValuePair<string, string> GetAccountsErrors(this AccountsErrors accountsErrors)
        {
            return accountsErrors switch
            {
                AccountsErrors.InvalidUserId => new KeyValuePair<string, string>(GetCodeFormat(1), InvalidUserMessage),
                AccountsErrors.PasswordNullOrEmpty => new KeyValuePair<string, string>(GetCodeFormat(2), PasswordNullOrEmptyMessage),
                AccountsErrors.PasswordToBig => new KeyValuePair<string, string>(GetCodeFormat(3), PasswordToBigMessage),
                AccountsErrors.ServiceBusQueueConfigurationNull => new KeyValuePair<string, string>(GetCodeFormat(4), ServiceBusQueueConfigurationNulLMessage),
                AccountsErrors.ServiceBusQueueConfigurationMessageToBig => new KeyValuePair<string, string>(GetCodeFormat(5), ServiceBusQueueConfigurationMessageToBigMessage),
                AccountsErrors.ProcessorQueueNotInstantiate => new KeyValuePair<string, string>(GetCodeFormat(6), ProcessorQueueNotInstantiateMessage),
                AccountsErrors.UserNotFound => new KeyValuePair<string, string>(GetCodeFormat(7), UserNotFoundMessage),
                AccountsErrors.RequestChangePasswordTokenAlreadyRegistered => new KeyValuePair<string, string>(GetCodeFormat(8), RequestChangePasswordTokenAlreadyRegisteredMessage),
                AccountsErrors.RequestChangePasswordTokenNotRegistered => new KeyValuePair<string, string>(GetCodeFormat(9), RequestChangePasswordTokenNotRegisteredMessage),
                AccountsErrors.InvalidUserEmail => new KeyValuePair<string, string>(GetCodeFormat(10), SearchUserEmailInvalidMessage),
                AccountsErrors.EmailAlreadyExists => new KeyValuePair<string, string>(GetCodeFormat(11), EmailAlreadyExistsMessage),
                AccountsErrors.RequestChangePasswordTokenExpired => new KeyValuePair<string, string>(GetCodeFormat(12), RequestChangePasswordTokenExpiredMessage),
                _ => new KeyValuePair<string, string>(GetCodeFormat(0), DefaultMessage)
            };
        }

        private static string GetCodeFormat(int index)
        {
            object i = index;
            return string.Format(CodeFormat, CodePrefix, i);
        }
    }
}