# ToolsPack.Thread

## ThrottledLoader0

Inspire from the [Throttling technique](https://codeburst.io/throttling-and-debouncing-in-javascript-b01cad5c8edf) in the javascript world.

Problem it try to solve

```CSharp
//A generic loader with zero argument
public delegate T Loader0<T>();

Loader0<int> lo = () => { read_database; return i; }

var result = lo(); // read_database every call
for (1..10000) { result = lo(); }  //BAD: call read_database 10000 times

ThrottledLoader0<int> tlo = new ThrottledLoader0<int>(lo, 500); //0.5s
var result = tlo.GetValue(); // read_database (at most once every 0.5s)
for (1..10000) { result = tlo.GetValue(); }  //GOOD: read_database only once every 0.5s
```

- `Loader0<T>()` is a function which take zero argument (no input) and return an object `T`.
- `ThrottledLoader0` is a decorator of the `Loader0`. It caches the last output of the `Loader0`. The cached value expired after 0.5s (in the example). Each time we ask for the output of the function `Loader0`, the `ThrottledLoader0` will return the cached value if it is not expired, otherwise it will re-compute the output value by invoking the `Loader0` again.
- `ThrottledLoader0` add the throttling effect on the function `Loader0` so that if you call this function 10000 times (to get the `T` output), you will only get the cached value in the decorator. The real function `Loader0` will be called only once every 0.5s

### ThrottledLoaderAsync0

```Csharp
// A generic loader with zero argument
public delegate Task<T> LoaderAsync0<T>();

LoaderAsync0<int> lo = async () => { await select_from_database; return i; }

var result = await lo(); // select_from_database every call
for (1..10000) { result = await lo(); }  //BAD: select_from_database 10000 times

ThrottledLoaderAsync0<int> tlo = new ThrottledLoaderAsync0<int>(lo, 500); //0.5s
var result = await tlo.GetValue(); // select_from_database at most once every 0.5s
for (1..10000) { result = await tlo.GetValueAsync(); }  //GOOD: select_from_database only once every 0.5s
```

## TimedLock

https://github.com/Haacked/TimedLock

```CSharp
using(TimedLock.Lock(obj, TimeSpan.FromSeconds(10)))
{
    //Thread safe operations
}
```

- The "synchronized code" will wait for other lock on `obj` free.
- `TimeOutException` if the lock acquiring is longer than 10 sec

## NamedLocker

```CSharp
static readonly NamedLocker<string> CustomerLocker = new NamedLocker<string>();
customerLocker.RunWithLock("Peter.Buy", () =>
{
	//synchronized code
}
```

- The "synchronized code" will wait for other "Peter.Buy"` key free.

## MultiNamedTimedLocker

```CSharp
static readonly MultiNamedTimedLocker<string> CustomerLocker = new MultiNamedTimedLocker<string>();

using (customerLocker.Lock(new[] {"peter", "david"}, 100))
{
	//synchronized code
}
```

- The "synchronized code" will wait until the `"peter"` and `"david"` key of the `CustomerLocker` object are free.
- After 100 mili-second of waiting: `TimeOutException`
