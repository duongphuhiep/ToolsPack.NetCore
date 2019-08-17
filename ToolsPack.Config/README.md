## ConfigReader

use it to read `app.config`

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
