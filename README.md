# Topiary C# Bindings

This package provides a wrapper for [topiary](https://github.com/peartreegames/topiary) functions.

Current topiary v0.8.0

## Setup

 - Add Topiary to your project with nuget or cloning this repo.
 - Compile your .topi file with `Story.Compile(string)` (always good to serialize the returns `byte[]` and store for runtime)
 - Create your OnDialogue and OnChoices Callbacks
   - `public delegate void OnDialogueCallback(Story story, Dialogue dialogue);`
   - `public delegate void OnChoicesCallback(Story story, Choice[] choices);`
 - Create a new Story with `var story = new Story(bytes, onDialogue, onChoices, logSeverity)`
 - Run your story until the end, calling `story.SelectContinue()` after every `onDialogue` and `story.SelectChoice(int)` after every `onChoices` that's invoked.
   - ```csharp
        try
        {
            story.Start();
            // this example assumes onDialogue calls story.SelectContinue() itself
            while (story.CanContinue) story.Run();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
     ```
     

## Examples

Check out the `Test/UnitTest.cs` for a very simple setup, as well as the [Topiary.Unity](https://github.com/peartreegames.topiary-unity).

## Functions

Topiary provides a `TopiAttribute`, simply add `[Topi(name)]` where "name" matches the extern variable name in your .topi file. 

Any function with 0-4 `TopiValue` parameters and return type `void` or `TopiValue` is valid.

```csharp
// .topi: extern const strPrint = |str| {}
//  will be overriden with the C# function
[Topi("strPrint")]
private static void StrPrint(TopiValue value)
{
    var str = value.String;
    Console.WriteLine($"StrPrint:: {str}");
}

// .topi: extern const sqr = |x| print("sqr: {x}")
//  will be overriden with the C# function
[Topi("sqr")]
private static TopiValue Sqr(TopiValue value)
{
    var i = value.Int;
    return new TopiValue(i * i);
}
```

Then you can bind all the functions with the attribute in the assembly with `story.BindFunctions(Assemblies[])`

## Values

Currently only the following value types are valid in the C# bindings (*Experiemental and requires deallocating memory):
 - Null
 - Bool
 - Float
 - String
 - List*
 - Map*
 - Set*
