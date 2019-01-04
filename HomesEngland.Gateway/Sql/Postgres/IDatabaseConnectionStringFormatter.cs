namespace HomesEngland.Gateway
{
    public interface IDatabaseConnectionStringFormatter
    {
        string BuildConnectionStringFromUrl(string databaseUrl);
    }
}