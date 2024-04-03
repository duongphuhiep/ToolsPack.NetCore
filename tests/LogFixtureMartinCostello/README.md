# Tutorial How to export xUnit's fixtures logging messages to the Test Ouput

It is common that a xUnit fixture is a [WebApplicationFactory](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0) which would start and run a full ASP.NET application.

And the application (your xUnit fixture) produces a lot of logs when a test is run.

It is difficult to see all these logs, because xUnit considers them as diagnostics messages, and send them to other output channel (not the same channel as the Tests Output).

This example project shows a solution to this problem. Both logs messages of the fixture (diagnostics messages) and of the Tests will be sent to the Tests Output.

![result](https://github.com/duongphuhiep/ToolsPack.NetCore/assets/1638594/43533d7a-e6ee-4c8d-9523-94e0440e91f6)

## How it works?

The fixture class has to implement `ITestOutputHelperAccessor` interface.
![fixture](https://github.com/duongphuhiep/ToolsPack.NetCore/assets/1638594/a500c7d3-4689-4fe0-82bf-86013fbd8e6d)

The tests classes have to "give" their `ITestOutputHelper` to the fixture class.
![test](https://github.com/duongphuhiep/ToolsPack.NetCore/assets/1638594/d0a96c3a-258c-44ff-b058-cd21f2fd9d39)
