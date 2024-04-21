# MacLoader

Namespace: PeartreeGames.Topiary

Represents a loader interface for loading and interacting with libraries.

```csharp
public class MacLoader : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid, System.IDisposable, ILoader
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [CriticalFinalizerObject](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.constrainedexecution.criticalfinalizerobject) → [SafeHandle](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.safehandle) → [SafeHandleZeroOrMinusOneIsInvalid](https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.safehandles.safehandlezeroorminusoneisinvalid) → [MacLoader](./peartreegames.topiary.macloader.md)  
Implements [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable), [ILoader](./peartreegames.topiary.iloader.md)

## Properties

### **IsInvalid**

```csharp
public bool IsInvalid { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  

### **IsClosed**

```csharp
public bool IsClosed { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  

## Constructors

### **MacLoader(Boolean)**

Represents a loader interface for loading and interacting with libraries.

```csharp
public MacLoader(bool ownsHandle)
```

#### Parameters

`ownsHandle` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  

## Methods

### **Load()**

Loads the library by calling the underlying native method dlopen.

```csharp
public SafeHandle Load()
```

#### Returns

[SafeHandle](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.safehandle)  
A SafeHandle object representing the loaded library.

### **Free(IntPtr)**

Frees the specified library handle.

```csharp
public bool Free(IntPtr ptr)
```

#### Parameters

`ptr` [IntPtr](https://docs.microsoft.com/en-us/dotnet/api/system.intptr)  
The pointer to the library handle.

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  
`true` if the library handle is successfully freed; otherwise, `false`.

### **GetProc(String)**

Retrieves the address of the specified function from the loaded library.

```csharp
public IntPtr GetProc(string name)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The name of the function to retrieve.

#### Returns

[IntPtr](https://docs.microsoft.com/en-us/dotnet/api/system.intptr)  
The address of the specified function if the function is found, or IntPtr.Zero if the function is not found.

### **ReleaseHandle()**

Releases the handle of the library.

```csharp
protected bool ReleaseHandle()
```

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  
`true` if the handle is successfully released; otherwise, `false`.
