# ToolsPack.String

## ArrayDisplayer

- Know to convert a `IEnummerable` to string in order do display in a log message

```CSharp
var arr = new string[1000] {"item1".."item1000"};
arr.Display().SeparatedBy("; ").MaxItems(4)
```

gives

```
{ item1; item2; item3; item4; ..and 996 (of 1000) more }
```

- If some items in the array are very long. We should limit the length of individual item with `MaxItemLength()` so that long items will be displayed with `...` ellipsis

```CSharp
var arr = new string[1000] {"Lorem ipsum kidda foom", "item2".."item1000"};
arr.Display().MaxItems(4).MaxItemLength(10)
```

gives

```
{ [[Lorem...]], item2, item3, item4, ..and 996 (of 1000) more }
```

- Fast performance it only iterate neccessary items once (complexity O(N))
- see more functionalities in code and test

## StopwatchDisplayer

convert `Stopwatch` to string

```CSharp
Stopwatch sw;
Console.WriteLine(sw.DisplayMili()); //get the display string in mili seconds "103 ms"
Console.WriteLine(sw.DisplayMicro()); //get the display string in micro seconds "103,000 mcs"
Console.WriteLine(sw.Display()); //automaticly choose a time unit (day, hour, minute, seconde..) to display
```

## Tips

For some serializable object which don't implement ToString(). Newtonsoft.Json can convert them to json. DO NOT use this technique on production. The reflection is bad for Perf..

```CSharp
Newtonsoft.Json.JsonConvert.SerializeObject(someObject);
```

## Ellipsis

```CSharp
ArrayDisplayer.DefaultEllipsis("1234567890", 4, "..."); //gives "1234..."
ArrayDisplayer.WordEllipsis("123 567 90", 5, "..."); //gives "123 567..."
```

## Utf8SealCalculator

```CSharp
string signature = Utf8SealCalculator.HMACSHA256("payload", "secret", Utf8SealCalculator.ToHex);
```
* Convert the payload string and the secret string to **byte[] tables** with a **Utf-8 encoder**
  * If the secret or the payload represents a "hexString" (for eg.: "1a94f6c5a9") then you will have to declare `secretIsHexString = true` or `payloadIsHexString = true` so that they will be treated as HexString or else they will be treated as normal text string by default.
* Use the secret byte[] table to hash the payload byte[] table with the **HMACSHA256 algorithm**
* the third parameter convert the hash result back to string. You can use for example:
  * `Utf8SealCalculator.ToHex`: format the hash result from byte[] to hexa value string
  * `Convert.ToBase64String`: format the hash result from byte[] to a base64 string


The result string is usually used as a seal or a signature to authenticate the payload content. 

Other supported hashing algorithms are:
* HMACSHA1 (not secure)
* HMACSHA256
* HMACSHA384
* HMACSHA512
* SHA1 (not secure)
* SHA256
* SHA384
* SHA512

## DiacriticsRemover

```CSharp
string message = DiacriticsRemover.RemoveDiacritics("où déjà aperçu la phénomène"); //returns "ou deja apercu la phenomene"
```

## CreateRandomString

```CSharp
string randomString = StringGenerator.CreateRandomString(5, 0, "abcdefghijklmnpqrstuvwxyz0123456789");
```
- Generate a random string of length 5 using random characters in `"abcdefghijklmnpqrstuvwxyz0123456789"`
- 0 is the length variable, 0-variable means that the length result is fix to 5.
- CreateRandomString(5, 3) will result a string with length variable between (5 and 8)

Remark:

1. A radom string is not suppose to replace the GUID because it got higher chance of colission. If you want a shorter Guid then check out the `ShortGuid` class
2. If you want to generate nice fake data such as Person name, Email, Product... then Checkout the the [Bogus](https://github.com/bchavez/Bogus) project. Example:

```CSharp
var walletGenerator = new Faker<Wallet>()
    .RuleFor(o => o.Gender, f => (int)f.PickRandom<Gender>())
    .RuleFor(o => o.FirstName, (f, o) => f.Name.FirstName((Gender)o.Gender))
    .RuleFor(o => o.LastName, (f, o) => f.Name.LastName())
    .RuleFor(o => o.Situation, f => f.Random.Int(0, 6))
    .RuleFor(o => o.IsBlocked, f => f.PickRandomParam(true, false))
    .RuleFor(o => o.Email, (f, o) => f.Internet.Email(o.FirstName, o.LastName))
    .RuleFor(o => o.Balance, f => f.Random.Decimal(0, 10000))
    .RuleFor(o => o.Name, (f, o) => o.FirstName.ToLower() + "." + o.LastName.ToLower() + "." + f.Random.String2(5))
    .RuleFor(o => o.CreationDate, f => f.Date.Past(3))
;
var w = walletGenerator.Generate();
```

## ShortGuid

A normal GUID (or UUID) is a random 128-bit, the standard Microsoft Guid class format it in base-16 so a length-32 string.
Why not format it in base-64 for a shorter (length-22) string? Here how to do it:

```CSharp
var g = Guid.NewGuid();

var shortGuid = g.ToShortGuid();
Assert.Equal(g, ShortGuid.Parse(shortGuid));

var shortGuidUrlFriendly = g.ToShortGuid(true);
Assert.Equal(g, ShortGuid.Parse(shortGuidUrlFriendly, true));
```
**Note:** 

1. if the ShortGuid (length-22) is still too long, you can create a even shorter unique id with [hashid](https://hashids.org/net/). Youtube is using this popular technique to create short + unique id for their videos. [Checkout this video](https://youtu.be/tSuwe7FowzE)

2. Use the [NewId](https://github.com/phatboyg/NewId) library if you want to generate a sequential (sortable) GUID. It will also help to reduce the [MS Sql Server index fragmentation](https://andrewlock.net/generating-sortable-guids-using-newid/)

## SqlServerConnectionStringBuilder

```Csharp
string connectionString = SqlServerConnectionStringBuilder.Build("localhost", "mydb", "root", "secretpassword");
```

## UriCombine

```Csharp
Assert.Equal("http://www.my.domain/relative/path", UriCombine.Join("http://www.my.domain/", "relative/path").ToString());
Assert.Equal("http://www.my.domain/absolute/path", UriCombine.Join("http://www.my.domain/something/other", "/absolute/path").ToString());
```

For more complex Uri manipulation, checkout the [Furl](https://flurl.dev/) project.

## Working with XmlDocument and XDocument

* `XDocument` is recommended over the old `XmlDocument`

Serialize a object to a `XDocument`

```Csharp
XDocument doc = XDocumentFactory.CreateDocFromXmlSerializer(o);
Console.WriteLine(xmlDoc.ToString());
```

Serialize a object to a `XmlDocument`

```Csharp
XmlDocument doc = XmlDocumentFactory.Create(o);
Console.WriteLine(doc.OuterXml);
```

Convert `XDocument` <=> `XmlDocument`

```Csharp
var xmlDoc = XmlDocumentFactory.Create(o);
var xDoc = XDocumentFactory.CreateDocFromXmlSerializer(o);
Console.WriteLine(xmlDoc.ToXDocument().ToString());
Console.WriteLine(xDoc.ToXmlDocument().OuterXml);
```

## Tips: checkout also

1. [Bogus](https://github.com/bchavez/Bogus): If you want to generate nice fake data such as Person name, Email, Product...

2. [Humanizer](https://github.com/Humanizr/Humanizer): If you want to generate nice human readable string from strings, enums, dates, times, timespans, numbers and quantities...


