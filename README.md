# ToolsPack: collection of useful codes targeting .NET Standard

My collection of many small useful code-snippets .net, wrapping in reusable [library on NuGet](https://www.nuget.org/profiles/duongphuhiep) with unobtrusive dependencies.

This repository won't got much updates because the codes are very stable through years of battle-testing on production hence nearly Zero-maintenance is needed.

Navigate to the `README.md` in each packages for more information.

## Demo

We'll connect to the database to get all "Payment Method"; feed them to a `List<string>`, and finally display this list, while evaluating time spent on each steps.

```csharp
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsPack.Sql;
using log4net;
using ToolsPack.Log4net;
using System.Collections.Generic;
using ToolsPack.Displayer;

namespace Payment.Tests
{
  [TestClass]
  public class CallDboTests
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof(CallDboTests));

    [TestInitialize]
    public void SetUp()
    {
      //Tell log4net to write all log to Console // see: ToolsPack.Log4net
      Log4NetQuickSetup.SetUpConsole();

      Log.Info("Init test"); //print to console: same as Console.WriteLine("Init test")
    }

    /// <summary>
    /// This example will make "select" in the database to get all Payment Method Name to a list
    /// then print the list to console.
    /// </summary>
    [TestMethod]
    public void MakeQuery()
    {
      var paymentMethods = new List<string>();

      //read the connection string from app.config or use the default value //see ToolsPack.Config
      var connectionString = ConfigReader.Read("connectionString", @"Data Source=192.168.0.1;Initial Catalog=toto;User ID=dev_user;Password=papapapap;");

      //setup a micro-benchmark, to mesure each step //see ToolsPack.Log4net
      using (var etw = ElapsedTimeWatcher.Create(Log, "MakeQuery"))
      {
        //Use ADO.NET helper to access the database //see ToolsPack.Sql
        using (var db = new AdoHelper(connectionString))
        {
          //we use 'etw' to log message and the benchmark
          etw.DebugFormat("Database connect OK, {0}", connectionString); //it will also display elapsed time to connect to database

          db.BeginTransaction(); //show how to make transaction (useless in this example)

          const string query = "select * from payments_methods where operation_type=@tpy";
          using (var reader = db.ExecDataReader(query, "@tpy", 0))
          {
            etw.Debug("Exec query OK"); //it will also display elapsed time to execute the query

            while (reader.Read())
            {
              //use another ADO.Net sugar helper to read cells value
              var name = reader.GetValue<string>("payment_method");

              paymentMethods.Add(name); //feed the list
            }
            etw.Debug("Collect data OK"); //it will also display elapsed time to read all the rows
          }

          db.Commit(); //show how to make transaction (useless in this example)
        }
      } // end of the micro-benchmark, it will display the total time spent in this block

      //Convert an Array/List to a readable string //see ToolsPack.Displayer
      Log.InfoFormat("Payment method found {0}", paymentMethods.Display());
    }
  }
}
```

### Console Output

```text
11:37:17,379 [INFO ] Init test [CallDboTests:24]
11:37:17,402 [DEBUG] Begin MakeQuery [CallDboTests:0]
11:37:17,695 [DEBUG] MakeQuery - 288412 mcs - Database connect OK, Data Source=192.168.0.1;Initial Catalog=toto;User ID=dev_user;Password=papapapap; [CallDboTests:0]
11:37:17,801 [DEBUG] MakeQuery - 106062 mcs - Exec query OK [CallDboTests:0]
11:37:17,812 [DEBUG] MakeQuery - 11381 mcs - Collect data OK [CallDboTests:0]
11:37:17,869 [INFO ] End MakeQuery : Total elapsed 462548 mcs [CallDboTests:0]
11:37:17,869 [INFO ] Payment method found { CARD, RIB, PRELEVEMENT, CHEQUE, VOILA, OHMYGOD } [CallDboTests:71]
```

Imagine How much line of codes you would have to write without help of [ToolsPack.NetCore]

## Remark

These codes snippets are collected from different sources, you won't see much unit tests in this repository because I'm too lazy to port them from the old repositories.
