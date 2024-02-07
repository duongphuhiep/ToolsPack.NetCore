using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsPack.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToolsPack.Config.Tests
{
    [TestClass()]
    public class ConfigFileLocationTests
    {
        [TestMethod()]
        public void FindTest()
        {
            Assert.IsNull(ConfigFileLocator.Find("toto.json"));

            var fullPathToMySettings = ConfigFileLocator.Find("mysettings.json");
            Assert.IsNotNull(fullPathToMySettings);
        }
    }
}