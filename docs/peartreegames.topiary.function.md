# Function

Namespace: PeartreeGames.Topiary

Represents a function that can be called dynamically.

```csharp
public class Function : System.IDisposable
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Function](./peartreegames.topiary.function.md)  
Implements [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable)

## Constructors

### **Function(Delegate)**

```csharp
public Function(Delegate del)
```

#### Parameters

`del` [Delegate](https://docs.microsoft.com/en-us/dotnet/api/system.delegate)  

## Methods

### **GetCallIntPtr()**

Returns the function pointer for the GetCallIntPtr method.

```csharp
public IntPtr GetCallIntPtr()
```

#### Returns

[IntPtr](https://docs.microsoft.com/en-us/dotnet/api/system.intptr)  
The function pointer for the GetCallIntPtr method.

### **Dispose()**

Disposes of the resources used by the Function instance.

```csharp
public void Dispose()
```

### **ToString()**

Converts the Function object to its string representation.

```csharp
public string ToString()
```

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
A string that represents the current Function object.

### **Call(IntPtr, Byte)**

Executes the delegate stored in the Function object.

```csharp
public TopiValue Call(IntPtr argPtr, byte count)
```

#### Parameters

`argPtr` [IntPtr](https://docs.microsoft.com/en-us/dotnet/api/system.intptr)  
A pointer to the arguments for the delegate.

`count` [Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)  
The number of arguments.

#### Returns

[TopiValue](./peartreegames.topiary.topivalue.md)  
The result of executing the delegate.

### **Create(MethodInfo)**

Creates a [Function](./peartreegames.topiary.function.md) object based on the given [MethodInfo](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.methodinfo).

```csharp
public static Function Create(MethodInfo method)
```

#### Parameters

`method` [MethodInfo](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.methodinfo)  
The [MethodInfo](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.methodinfo) representing the method.

#### Returns

[Function](./peartreegames.topiary.function.md)  
A new instance of [Function](./peartreegames.topiary.function.md) created from the method.

#### Exceptions

[NotSupportedException](https://docs.microsoft.com/en-us/dotnet/api/system.notsupportedexception)  
Thrown when the number of parameters is not supported.
