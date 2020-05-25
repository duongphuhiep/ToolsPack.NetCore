using System;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace ToolsPack.Config
{
    /// <summary>
    /// Locate a config file. Example: ConfigFileLocator.Find("appsettings.json");
    /// </summary>
    public static class ConfigFileLocator
    {
        /// <summary>
        /// Find a config file in certain susceptible locations:
        /// the current folder, Binary folder (RelativeSearchPath), BaseDirectory, AssemblyLocation..
        /// return full path to the config file or null if not found
        /// </summary>
        public static string Find(string configFileName)
        {
            //find in the current folder first! most of time it will returned immediately here
            {
                var f = new FileInfo(configFileName);
                if (f.Exists) return f.FullName;
            }

            //find in basic place where we don't need to compute the folder location
            {
                string[] foldersToSearch = new[]
                {
                    //the /bin folder for Web Apps
                    AppDomain.CurrentDomain.RelativeSearchPath,

                    //the exe folder for WinForms, Consoles, Windows Services
                    AppDomain.CurrentDomain.BaseDirectory,

                    Environment.CurrentDirectory,
                    AppContext.BaseDirectory
                };

                foreach (var folder in foldersToSearch)
                {
                    if (!string.IsNullOrEmpty(folder))
                    {
                        var f = new FileInfo(Path.Combine(folder, configFileName));
                        if (f.Exists) return f.FullName;
                    }
                }
            }

            //Last resort! find in other place (need to compute the folder locations)
            {
                var folder = Directory.GetCurrentDirectory();
                if (!string.IsNullOrEmpty(folder))
                {
                    var f = new FileInfo(Path.Combine(folder, configFileName));
                    if (f.Exists) return f.FullName;
                }

                folder = typeof(ConfigFileLocator).Assembly.Location;
                if (!string.IsNullOrEmpty(folder))
                {
                    var f = new FileInfo(Path.Combine(folder, configFileName));
                    if (f.Exists) return f.FullName;
                }
            }

            //We didn't find anything
            return null;
        }
    }
}
