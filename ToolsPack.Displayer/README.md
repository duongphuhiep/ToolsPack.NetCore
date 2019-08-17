# ToolsPack.Displayer

- No dependencies
- Use to display some C# object.

## ArrayDisplayer

- Know to convert a IEnummerable to string in order do display in a log message

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
