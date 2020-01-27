using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsPack.Samba;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToolsPack.Samba.Tests
{
    [TestClass()]
    public class FileStorageSettingTests
    {
        [TestMethod()]
        public void BasePathGetTest()
        {
            var f = new FileStorageSetting
            {
                RemoteLocation = @"\\10.20.9.10\carte_identite",
                BaseLocation = @"app\dev"
            };
            Assert.AreEqual(f.BasePath, @"\\10.20.9.10\carte_identite\app\dev");
        }
    }
}