using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Bench;

// Console.WriteLine(GenerateBase64Guid.S1());
// Console.WriteLine(GenerateBase64Guid.S2());
// Console.WriteLine(GenerateBase64Guid.S3());
// Console.WriteLine(GenerateBase64Guid.S4());
// Console.WriteLine(GenerateBase64Guid.S7());
//  return;

BenchmarkRunner.Run<MyBenchmark>();

[MemoryDiagnoser]
public class MyBenchmark
{
    [Benchmark]
    public void V7()
    {
        GenerateBase64Guid.V7();
    }
    [Benchmark]
    public void V11()
    {
        GenerateBase64Guid.V11();
    }
    [Benchmark]
    public void V12()
    {
        GenerateBase64Guid.V12();
    }
    
    [Benchmark]
    public void S1()
    {
        GenerateBase64Guid.S1();
    }
    [Benchmark]
    public void S2()
    {
        GenerateBase64Guid.S2();
    }
    [Benchmark]
    public void S3()
    {
        GenerateBase64Guid.S3();
    }
    [Benchmark]
    public void S4()
    {
        GenerateBase64Guid.S4();
    }
    [Benchmark]
    public void S7()
    {
        GenerateBase64Guid.S7();
    }
}


/*
   | Method | Mean      | Error    | StdDev   | Gen0   | Allocated |
   |------- |----------:|---------:|---------:|-------:|----------:|
   | V1     | 462.41 ns | 2.295 ns | 2.147 ns | 0.0267 |     225 B |
   | V2     | 448.32 ns | 6.500 ns | 6.080 ns | 0.0086 |      72 B |
   | V4     | 449.83 ns | 4.174 ns | 3.904 ns | 0.0134 |     112 B |
   | V5     | 442.72 ns | 2.595 ns | 2.427 ns | 0.0172 |     144 B |
   | V6     | 154.87 ns | 1.119 ns | 1.047 ns | 0.0172 |     144 B |
   | V7     | 144.94 ns | 1.227 ns | 1.148 ns | 0.0086 |      72 B |
   | V8     | 686.99 ns | 5.534 ns | 4.906 ns | 0.0172 |     144 B |
   | V9     |  49.61 ns | 0.311 ns | 0.276 ns | 0.0172 |     144 B |
   | V11    |  27.78 ns | 0.215 ns | 0.201 ns | 0.0143 |     120 B |
   
   | V7     | 142.26 ns | 0.848 ns | 0.752 ns | 0.0086 |      72 B |
   | V11    |  27.85 ns | 0.175 ns | 0.155 ns | 0.0143 |     120 B |
   | V12    |  34.03 ns | 0.260 ns | 0.231 ns | 0.0249 |     208 B |
   | S1     | 162.21 ns | 1.228 ns | 1.149 ns | 0.0172 |     144 B |
   | S2     | 737.71 ns | 3.545 ns | 3.142 ns | 0.0229 |     192 B |
   | S3     | 138.96 ns | 2.729 ns | 2.552 ns | 0.0229 |     192 B |
   | S4     | 172.95 ns | 1.403 ns | 1.312 ns | 0.0172 |     144 B |
   | S7     | 171.72 ns | 0.844 ns | 0.748 ns | 0.0229 |     192 B |
   
*/