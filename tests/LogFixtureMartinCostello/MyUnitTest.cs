using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace ToolsPack.Logging.Tests;

public class MyUnitTest : IDisposable, IClassFixture<MyFixture>
{
    private readonly ILogger<MyUnitTest> _logger;
    private readonly MyFixture _myFixture;

    public MyUnitTest(ITestOutputHelper testOutputHelper, MyFixture myFixture)
    {
        _logger = myFixture.LoggerFactory.CreateLogger<MyUnitTest>();

        _logger.LogInformation("Before setup the OutputHelper in the fixture, log messages won't appeared in the Test Output ");
        myFixture.OutputHelper = testOutputHelper;
        _logger.LogInformation("After setup the OutputHelper in the fixture, log messages will appeared!");
        _myFixture = myFixture;
    }

    public void Dispose()
    {
        _logger.LogInformation("TearDown MyUnitTest");
    }

    [Fact]
    public void MyTest()
    {
        Console.WriteLine("we won't see this Console message in the Test Output");
        _myFixture.CallApplicationCodes();
        _logger.LogInformation("Test message");
    }
}
