using System;
using Kuva.Accounts.Business.Logging.Domains;

namespace Kuva.Accounts.Business.Logging.Interfaces
{
    public interface IKuvaLogger
    {
        void LogInfo(OperationValue operationValue);
        void LogError(Exception exceptions, OperationValue operation);
    }
}
