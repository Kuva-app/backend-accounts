namespace Kuva.Accounts.Repository
{
    internal struct Constants
    {
        internal const string ServiceSchemaName = "accounts";
        internal const string ConnectionStringName = "KuvaConnection";
        internal const string ConnectionStringNullExceptionParams = "_configuration.GetConnectionString(Constants.ConnectionStringName)";
        internal const string MigrationAssemblyName = "Kuva.Accounts.Repository";
    }
}