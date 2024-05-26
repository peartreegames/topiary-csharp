# Dialogue

Namespace: PeartreeGames.Topiary

Represents a dialogue instance.

```csharp
public class Dialogue : System.IDisposable
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Dialogue](./peartreegames.topiary.dialogue.md)  
Implements [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable)

## Properties

### **IsValid**

Gets a value indicating whether the Dialogue instance is valid.

```csharp
public bool IsValid { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  

**Remarks:**

This property returns `true` if the internal pointer `_vmPtr` is not zero;
 otherwise, it returns `false`.

### **Library**

Represents a library that provides functionality for creating and managing dialogues.

```csharp
public Library Library { get; }
```

#### Property Value

[Library](./peartreegames.topiary.library.md)  

### **CanContinue**

Gets a value indicating whether the dialogue can continue.

```csharp
public bool CanContinue { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  
`true` if the dialogue can continue; otherwise, `false`.

### **IsWaiting**

Gets a value indicating whether the dialogue is waiting for user input.

```csharp
public bool IsWaiting { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  

## Constructors

### **Dialogue(Byte[], OnLineCallback, OnChoicesCallback, Severity)**

Represents a dialogue.

```csharp
public Dialogue(Byte[] source, OnLineCallback onLine, OnChoicesCallback onChoices, Severity severity)
```

#### Parameters

`source` [Byte[]](https://docs.microsoft.com/en-us/dotnet/api/system.byte)  

`onLine` [OnLineCallback](./peartreegames.topiary.dialogue.onlinecallback.md)  

`onChoices` [OnChoicesCallback](./peartreegames.topiary.dialogue.onchoicescallback.md)  

`severity` [Severity](./peartreegames.topiary.library.severity.md)  

## Methods

### **Dispose()**

Releases all resources used by the Dialogue object.

```csharp
public void Dispose()
```

### **Compile(String, Severity)**

Compile a ".topi" file into bytes.
 Should be saved to a ".topib" file
 Will precalculate the required capacity

```csharp
public static Byte[] Compile(string fullPath, Severity severity)
```

#### Parameters

`fullPath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The file absolute path

`severity` [Severity](./peartreegames.topiary.library.severity.md)  
Log severity

#### Returns

[Byte[]](https://docs.microsoft.com/en-us/dotnet/api/system.byte)  
Compiled bytes

### **Compile(String, Int64, Severity)**

Compile a ".topi" file into bytes.
 Should be saved to a ".topib" file

```csharp
public static Byte[] Compile(string fullPath, long capacity, Severity severity)
```

#### Parameters

`fullPath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The file absolute path

`capacity` [Int64](https://docs.microsoft.com/en-us/dotnet/api/system.int64)  
Capacity for the bytecode output

`severity` [Severity](./peartreegames.topiary.library.severity.md)  
Log severity

#### Returns

[Byte[]](https://docs.microsoft.com/en-us/dotnet/api/system.byte)  
Compiled bytes

### **Start(String)**

Start the Dialogue

```csharp
public void Start(string bough)
```

#### Parameters

`bough` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
Optional: The bough path where the conversation will start.
 If non provided, first bough in the file will be used

### **Run()**

Run the Dialogue until the next Dialogue or Choice

```csharp
public void Run()
```

#### Exceptions

[Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)  

### **Continue()**

Continue the Dialogue.

```csharp
public void Continue()
```

### **SelectChoice(Int32)**

Select a Choice.

```csharp
public void SelectChoice(int index)
```

#### Parameters

`index` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)  
The index of the choice selected

### **SaveState()**

Save current Dialogue State to JSON
 Will precalculate the necessary size
 Should merge the resulting JSON with the Game Root JSON State

```csharp
public string SaveState()
```

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  

### **SaveState(Int64)**

Save current Dialogue State to JSON
 Should merge the resulting JSON with the Game Root JSON State

```csharp
public string SaveState(long capacity)
```

#### Parameters

`capacity` [Int64](https://docs.microsoft.com/en-us/dotnet/api/system.int64)  
The maximum size of bytes to allocate

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  

### **LoadState(String)**

Load a JSON state into the Dialogue

```csharp
public void LoadState(string json)
```

#### Parameters

`json` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
JSON string

### **GetValue(String)**

Retrieve the current value of any Global variable in the dialogue

```csharp
public TopiValue GetValue(string name)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The name of the variable

#### Returns

[TopiValue](./peartreegames.topiary.topivalue.md)  
TopiValue.nil if not found

### **DestroyValue(TopiValue&)**

Destroy a reference value in unmanaged memory
 NOTE: This should be removed in future versions

```csharp
public void DestroyValue(TopiValue& value)
```

#### Parameters

`value` [TopiValue&](./peartreegames.topiary.topivalue&.md)  
The value to be destroyed

### **Subscribe(String, Subscriber)**

Subscribe to when a Global variable changes

```csharp
public bool Subscribe(string name, Subscriber callback)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The name of the variable

`callback` [Subscriber](./peartreegames.topiary.delegates.subscriber.md)  
The callback to be executed on change

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  

### **Unsubscribe(String, Subscriber)**

Unsubscribe when a Global variable changes

```csharp
public bool Unsubscribe(string name, Subscriber callback)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The name of the variable

`callback` [Subscriber](./peartreegames.topiary.delegates.subscriber.md)  
The callback that was passed into Subscribe

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  

### **Set(String, Boolean)**

Set an Extern variable to a bool value

```csharp
public void Set(string name, bool value)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The name of the variable

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  
The value to set

### **Set(String, Single)**

Set an Extern variable to a float value

```csharp
public void Set(string name, float value)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The name of the variable

`value` [Single](https://docs.microsoft.com/en-us/dotnet/api/system.single)  
The value to set

### **Set(String, String)**

Set an Extern variable to a float value

```csharp
public void Set(string name, string value)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The name of the variable

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The value to set

### **Set(String, Function, Byte)**

Set a Global Extern variable to a function value
 Note: It is easier to use the TopiAttribute instead with the BindFunctions method
 However this is kept in case you need more control

```csharp
public void Set(string name, Function function, byte arity)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The name of the variable

`function` [Function](./peartreegames.topiary.function.md)  
The value to set

`arity` [Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)  
The number of parameters the function accepts

### **Unset(String)**

Set an Extern variable to a nil value

```csharp
public void Unset(string name)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The name of the variable

### **BindFunctions(IEnumerable&lt;Assembly&gt;)**

Bind all TopiAttribute functions within the given Assemblies
 Functions must be of type "Func&lt;TopiValue[,TopiValue,TopiValue,TopiValue,TopiValue]&gt;"
 or "Action[&lt;TopiValue,TopiValue,TopiValue,TopiValue&gt;]"
 See [Function](./peartreegames.topiary.function.md)

```csharp
public void BindFunctions(IEnumerable<Assembly> assemblies)
```

#### Parameters

`assemblies` [IEnumerable&lt;Assembly&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)  
