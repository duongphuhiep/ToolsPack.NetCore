# ToolsPack.Blazor

Blazorify certain HTML tag to add or enhance their capability:

* `DebouncedInput` ~ `<input>`
* `Dialog` ~ `<dialog>`

For other components need, I recommend <https://daisyui.com/> for your Blazor application.


## Safely FireAndForget a task

```csharp
Task task;
task.FireAndForget(); //error will be write to the Console.
task.FireAndForget(logger); //use the logger to log error
```