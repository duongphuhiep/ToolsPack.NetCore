using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace ToolsPack.Samba.Tests
{
    [TestClass()]
    public class CifsConnectionManagerTests
    {
        Microsoft.Extensions.Logging.ILogger log;

        [TestInitialize()]
        public void SetUp()
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddLog4Net(); //load log4net.config by default
            log = loggerFactory.CreateLogger("T");
            log.LogDebug("Init test");
        }

        //[TestMethod()] skip this test
        public void CifsConnectionManagerTest()
        {
            log.LogInformation("Start test");

            //in this example we will try to access to the file @"\\10.20.30.40\carte_identity\mb\01\doc.pdf";

            //maybe the connection is already etablised (on the machine by other app) so we will just return the content of the file
            if (File.Exists(@"\\10.20.30.40\carte_identity\mb\01\doc.pdf"))
            {
                File.ReadAllBytesAsync(@"\\10.20.30.40\carte_identity\mb\01\doc.pdf").GetAwaiter().GetResult();
                return;
            }

            //we didn't entered the above "if" So maybe the connection is not etablished or the file really not exist on the remote disk

            //we will try to etablised the connection first

            FileStorageSetting setting = new FileStorageSetting
            {
                RemoteLocation = @"\\10.20.30.40\carte_identity",
                BaseLocation = @"mb",
                Login = "hduong",
                Password = "secret",
            };
            //CifsConnectionManagerFactory give a unique instance of a CifsConnectionManager correspond to the setting
            CifsConnectionManager conn = CifsConnectionManagerFactory.GetOrCreate(setting);

            //try to connect to the RemoteLocation 3 times. In the end it will throw NetworkDiskException if failed
            conn.Connect().GetAwaiter().GetResult();

            //Here the Connect() function didn't throw the NetworkDiskException. It means that the connection was succesfully etablished

            //whenever you want to check the connection status, you can always call the CheckConnectionAlive() function
            string err = conn.CheckConnectionAlive();
            if (err != null)
            {
                //the existence of the RemoteLocation\test.txt is not detected and attempt to create this file is failed
                throw new Exception("Connection is dead: here is the result of CheckConnectionAlive: " + err);
            }

            //you can use BasePath to access to other files relative to this path. BasePath is just (RemoteLocation + BaseLocation)
            var pathToFile = Path.Combine(setting.BasePath, @"01\doc.pdf"); //it will give "\\10.20.30.40\carte_identity\mb\01\doc.pdf"

            //the connection to the remote disk is alive but the file doc.pdf might not exist on the remote disk for real
            if (File.Exists(pathToFile))
            {
                //return the content of the file
                File.ReadAllBytesAsync(pathToFile).GetAwaiter().GetResult();
            }
            else
            {
                throw new Exception("No problem with the connection But the file doc.pdf really does not exist on the remote server");
            }
        }
    }
}