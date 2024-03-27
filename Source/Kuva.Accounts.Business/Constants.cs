using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Kuva.Accounts.Business
{
    internal static class Constants
    {
        internal const string UserPasswordSalt = "577F3020-F8AD-4AE6-8AA4-51721D0622D6";
        internal const int ConfirmationCodeMaxLenght = 8;
        internal const string ConfirmationCodeChars = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
        internal const string HashSecurityKey = "AFGKIEU33";
        internal const string RequestChangePasswordQueueName = "mail-change-password";
        internal const double RequestChangePasswordExpireHours = 1;
        internal const string SettingsElasticUrlString = "ElasticConfiguration:Uri";

        internal static string GetAssemblyFileVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersion = FileVersionInfo.GetVersionInfo(assembly. Location);
            return fileVersion. FileVersion;
        }
        
        internal static string GetElasticIndex(IConfiguration configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var elasticSearchIndex = configuration["ElasticConfiguration:Index"] ?? Assembly.GetExecutingAssembly().GetName().Name;
            elasticSearchIndex = elasticSearchIndex?.ToLower().Replace(".", "-");
            if (string.IsNullOrEmpty(elasticSearchIndex))
                return null;
            var index = $"{elasticSearchIndex.ToLower()}-{environment.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}";
            return index;
        }
    }
}
