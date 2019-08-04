using System;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace ToolsPack.Log4net.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            ILog Log = LogManager.GetLogger(typeof(Program));

            ILoggerRepository repository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());
            BasicConfigurator.Configure(repository);
            Log.Info("Something");
            Console.WriteLine("Finished");

            Console.ReadLine();
        }
    }
}
