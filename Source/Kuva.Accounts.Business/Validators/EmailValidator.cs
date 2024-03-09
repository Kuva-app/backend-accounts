using System.Text.RegularExpressions;

namespace Kuva.Accounts.Business.Validators
{
    public sealed class EmailValidator : IValidator
    {
        
        private const string Pattern =
            @"(?x)\b (?<![%+_$.-]) [a-z0-9](?:[a-z0-9] | [%+_$.-][a-z0-9])*@[a-z0-9](?:[a-z0-9] | [._-][a-z0-9])*\.[a-z]{2,6}\b";
        
        private EmailValidator()
        {
            _regex = new Regex(Pattern);
        }

        private readonly Regex _regex;
        private static EmailValidator _instance;
        
        public static EmailValidator Shared => _instance ??= new EmailValidator();

        public bool IsTrue<T>(T value)
        {
            if (!(value is string email))
                return false;
            return _regex.Match(email).Success;
        }
    }
}