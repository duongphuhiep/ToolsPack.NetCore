# ToolsPack.Config

## Read `app.config`

use it to read `app.config`. It won't work for ASP.NET Core application.

Example `app.config` of your application

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
	<appSettings>
		<add key="connectionString" value="Server=localhost;Database=foo"/>
		<add key="activePingService" value="true"/>
		<add key="pollIteration" value="100"/>
	</appSettings>
</configuration>
```

You can read these value in your C# application

```CSharp
ConfigReader.Read<string>("connectionString", "a default value if config not found");
ConfigReader.Read<bool>("activePingService", false);
ConfigReader.Read<int>("pollIteration", -1);
```

## Locate a config file

```CSharp
string fullPathToAppSettings = ConfigFileLocator.Find("appsettings.json");
```

Locate a configuration file in certain susceptible locations:

* Current directory
* `AppDomain.CurrentDomain.RelativeSearchPath`: the /bin folder for Web Apps
* `AppDomain.CurrentDomain.BaseDirectory`: the exe folder for WinForms, Consoles, Windows Services
* `Environment.CurrentDirectory`
* `AppContext.BaseDirectory`
* `Directory.GetCurrentDirectory()`
* `Assembly.Location`

Return the full path to the configuration file or null if not found

Usage example in a **console** application:
```CSharp
var builder = new ConfigurationBuilder()
	.AddJsonFile(ConfigFileLocator.Find("appsettings.json")); //add package Microsoft.Extensions.Configuration.Json
var configuration = builder.Build();
var settings = configuration.GetValue<Settings>("App");
```


