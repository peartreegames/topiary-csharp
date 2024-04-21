# ILoader

Namespace: PeartreeGames.Topiary

Represents a loader interface for loading and interacting with libraries.

```csharp
public interface ILoader
```

## Methods

### **Load()**

Loads the library.

```csharp
SafeHandle Load()
```

#### Returns

[SafeHandle](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.safehandle)  
A [SafeHandle](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.safehandle) representing the loaded library.

#### Exceptions

T:System.ComponentModel.Win32Exception  
Thrown if the library failed to load.

### **Free(IntPtr)**

Frees the specified library handle.

```csharp
bool Free(IntPtr ptr)
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
IntPtr GetProc(string name)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The name of the function to retrieve.

#### Returns

[IntPtr](https://docs.microsoft.com/en-us/dotnet/api/system.intptr)  
The address of the specified function if the function is found, or IntPtr.Zero if the function is not found.
