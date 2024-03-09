using System;
using System.Collections.Generic;
using Kuva.Accounts.Business.Logging.Domains;
using Kuva.Accounts.Business.Logging.Interfaces;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;

#nullable enable
namespace Kuva.Accounts.Business.Logging
{
    internal class KuvaElasticSearchLogger : IKuvaLogger
    {
        public void LogInfo(OperationValue operation)
        {
            var properties = new List<LogEventProperty>
            {
                new("operation", operation)
            };
            var messageTemplate = new MessageTemplateParser().Parse("info operationValue {Operation}");
            var newLogEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, messageTemplate,
                properties);
            Log.Write(newLogEvent);
        }

        public void LogError(Exception exception, OperationValue operation)
        {
            var properties = new List<LogEventProperty>
            {
                new("operation", operation)
            };
            var messageTemplate = new MessageTemplateParser().Parse("error {Exception} with operation {Operation}");
            var newLogEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Error, exception, messageTemplate,
                properties);
            Log.Write(newLogEvent);
        }
    }
}
