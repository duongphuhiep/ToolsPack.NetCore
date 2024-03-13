namespace ToolsPack.Logging.NUnitTests;

public class Tests
{
    private int _sharedNumber;

    public Tests()
    {
        Console.WriteLine("Test is created");
    }
    [SetUp]
    public void Setup()
    {
        Console.WriteLine("setup");
    }

    [TearDown]
    public void TearDown()
    {
        Console.WriteLine("TearDown");
    }

    [TestCase("A")]
    [TestCase("B")]
    public void Test1(string name)
    {
        _sharedNumber++;
        Console.WriteLine($"Test1 {name} {_sharedNumber}");
        Console.Write("Foo {0}", 1);
    }

    [TestCase("A")]
    [TestCase("B")]
    public void Test2(string name)
    {
        _sharedNumber += 5;
        Console.WriteLine($"Test2 {name} {_sharedNumber}");
    }
}