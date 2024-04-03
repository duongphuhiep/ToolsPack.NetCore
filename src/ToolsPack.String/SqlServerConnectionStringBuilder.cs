using System.Text.RegularExpressions;

namespace ToolsPack.String;

/// <summary>
/// A convenient <a href="https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnectionstringbuilder?view=dotnet-plat-ext-8.0">SqlConnectionStringBuilder</a>
/// </summary>
public static class QuickSqlServerConnectionStringBuilder
{
    public static string Build(string host, string dbname, string login, string password)
        => $"Data Source={host};Initial Catalog={dbname};User ID={login};Password='{password}';Trust Server Certificate=Yes;";

    public static string Build(string host, string dbname, string login, string password, string trustServerCertificate, int connectionTimeOutInSecond)
        => $"Data Source={host};Initial Catalog={dbname};User ID={login};Password='{password}';Trust Server Certificate=Yes;Connection Timeout={connectionTimeOutInSecond}";

    /// <summary>
    /// Add or replace connection timeout value in a connection string
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="connectionTimeOutInSecond"></param>
    /// <returns></returns>
    public static string ChangeTimeOut(string connectionString, int connectionTimeOutInSecond)
    {
        string tagPattern = @"\s*Connection\s*Timeout\s*=\s*[0-9]*\s*\;";
        string newValueToReplaceWith = $"Connection Timeout = {connectionTimeOutInSecond};";

        if (!Regex.IsMatch(connectionString, tagPattern))
        {
            return connectionString + $"Connection Timeout = {connectionTimeOutInSecond};";
        }
        else
        {
            return Regex.Replace(connectionString, tagPattern, newValueToReplaceWith);
        }
    }
}