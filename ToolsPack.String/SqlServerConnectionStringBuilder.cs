using System.Globalization;
using System.Text.RegularExpressions;

namespace ToolsPack.String
{
    public static class SqlServerConnectionStringBuilder
    {
        static public string Build(string host, string dbname, string login, string password, string connectionStringTemplate = "Data Source={ip};Initial Catalog={dbname};User ID={login};Password={password};")
        {
            return connectionStringTemplate
                        .Replace("{ip}", host)
                        .Replace("{dbname}", dbname)
                        .Replace("{login}", login)
                        .Replace("{password}", "'" + password + "'");
        }

        public static string Build(string host, string dbname, string login, string password, int connectionTimeOutInSecond, string connectionStringTemplate = "Data Source={ip};Initial Catalog={dbname};User ID={login};Password={password};  Connection Timeout={timeout}")
        {
            return connectionStringTemplate
                        .Replace("{ip}", host)
                        .Replace("{dbname}", dbname)
                        .Replace("{login}", login)
                        .Replace("{password}", "'" + password + "'")
                        .Replace("{timeout}", connectionTimeOutInSecond.ToString(CultureInfo.InvariantCulture));
        }

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
}