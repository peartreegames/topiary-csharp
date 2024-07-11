# Archived

I've archived this repository for now. Too many concessions were given
to make Unity work, it made more sense to keep them separate and duplicate things as needed.
If someone ever needs this for their own engine I'll leave it here to help
get them started. Though I'd recommend going back through the history, as there were
more modern C# uses and kept things a lot cleaner.

# Topiary C# Bindings

This package provides a wrapper for [topiary](https://github.com/peartreegames/topiary) functions.

Current topiary v0.12.0

## Setup

 - Add Topiary to your project with nuget or cloning this repo.
 - Compile your .topi file with `Dialogue.Compile(string)` (always good to serialize the returns `byte[]` and store for runtime)
 - Create your OnLine and OnChoices Callbacks
   - `public delegate void OnLineCallback(Dialogue dialogue, Line dialogue);`
   - `public delegate void OnChoicesCallback(Dialogue dialogue, Choice[] choices);`
 - Create a new Dialogue with `var dialogue = new Dialogue(bytes, onLine, onChoices, logSeverity)`
 - Run your dialogue until the end, calling `dialogue.SelectContinue()` after every `onLine` and `dialogue.SelectChoice(int)` after every `onChoices` that's invoked.
 
```csharp
try
{
    dialogue.Start();
    // this example assumes onLine callback calls dialogue.SelectContinue() itself
    while (dialogue.CanContinue) dialogue.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
}
```
     

## Examples

Check out the `Test/UnitTest.cs` for a very simple setup, as well as the [Topiary.Unity](https://github.com/peartreegames.topiary-unity).

## Functions

Topiary provides a `TopiAttribute`, simply add `[Topi(name, argCount)]` where "name" matches the extern variable name in your .topi file
and "argCount" is the number of arguments. 

Any static function with `IntPtr, byte` arguments and return type `TopiValue` is valid.
Originally functions were wrapped and allowed for `TopiValue` arguments to hide the IntPtr
and CreateArgs requirements, however Unity (the main use case for this package) will not work
with this work flow. So a more manual approcate is required.

```csharp
// .topi: extern const strPrint = |str| {}
//  will be overriden with the C# function
[Topi("strPrint", 1)]
private static TopiValue StrPrint(IntPtr argsPtr, byte count)
{
    var args = TopiValue.CreateArgs(argsPtr, count);
    var str = args[0].String;
    Console.WriteLine($"StrPrint:: {str}");
    return default;
}

// .topi: extern const sqr = |x| print("sqr: {x}")
//  will be overriden with the C# function
[Topi("sqr", 1)]
private static TopiValue Sqr(IntPtr argsPtr, byte count)
{
    var args = TopiValue.CreateArgs(argsPtr, count);
    var i = args[0].Int;
    return new TopiValue(i * i);
}
```

Then once you create your dialogue you can bind each function with `dialogue.Set(StrPrint);`

## Values

Currently only the following value types are valid in the C# bindings (*Experimental, readonly, and requires deallocating memory):
 - Null
 - Bool
 - Float
 - String
 - List*
 - Map*
 - Set*
