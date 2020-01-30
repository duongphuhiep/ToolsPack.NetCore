﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsPack.Samba;
using System;
using log4net;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Logging;
using ToolsPack.Log4net;
using log4net.Repository;

namespace ToolsPack.Samba.Tests
{
    [TestClass()]
    public class CifsConnectionManagerTests
    {
        Microsoft.Extensions.Logging.ILogger log;
        
        [TestInitialize()]
        public void SetUp()
        {
            Console.WriteLine("init test");
            ILoggerRepository loggerRepo = log4net.LogManager.GetRepository("G");
            Log4NetQuickSetup.SetUpConsole(Log4NetQuickSetup.GetSimplePattern(), loggerRepo);
            //Log4NetQuickSetup.SetUpFile(Log4NetQuickSetup.GetSimplePattern(), loggerRepo);
            log = new Log4NetLogger(new Log4NetProviderOptions {LoggerRepository = "G"});
        }

        [TestMethod()]
        public async Task<byte[]> CifsConnectionManagerTest()
        {
            Console.WriteLine("Start test console");
            log.LogInformation("Start test");
            
            //in this example we will try to access to the file @"\\10.20.30.40\carte_identity\mb\01\doc.pdf";

            //maybe the connection is already etablised (on the machine by other app) so we will just return the content of the file
            if (File.Exists(@"\\10.20.30.40\carte_identity\mb\01\doc.pdf"))
            {
                return await File.ReadAllBytesAsync(@"\\10.20.30.40\carte_identity\mb\01\doc.pdf");
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
            CifsConnectionManager conn = CifsConnectionManagerFactory.GetOrCreate(setting, log);

            //try to connect to the RemoteLocation 3 times. In the end it will throw NetworkDiskException if failed
            await conn.Connect();

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
                return await File.ReadAllBytesAsync(pathToFile);
            }
            else
            {
                throw new Exception("No problem with the connection But the file doc.pdf really does not exist on the remote server");
            }
        }
    }
}