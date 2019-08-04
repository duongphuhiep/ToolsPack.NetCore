using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository;

namespace ToolsPack.Log4net.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            ILog Log = LogManager.GetLogger(typeof(Program));

            Log4NetQuickSetup.SetUpFile("~/tmp/logs/a.log", Log4NetQuickSetup.GetSimplePattern());

            Log.Info("Something");
            var pp = Directory.GetCurrentDirectory();
            Console.WriteLine(pp);

            Console.ReadLine();
        }
    }
}
