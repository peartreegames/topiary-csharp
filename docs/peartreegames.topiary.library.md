# Library

Namespace: PeartreeGames.Topiary

Represents a library of functions and utilities for working with dialogue systems.

```csharp
public class Library : System.IDisposable
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Library](./peartreegames.topiary.library.md)  
Implements [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable)

## Fields

### **CreateVm**

```csharp
public CreateVmDelegate CreateVm;
```

### **DestroyVm**

```csharp
public DestroyVmDelegate DestroyVm;
```

### **Start**

```csharp
public StartDelegate Start;
```

### **CalculateCompileSize**

```csharp
public CalculateCompileSizeDelegate CalculateCompileSize;
```

### **Compile**

```csharp
public CompileDelegate Compile;
```

### **Run**

```csharp
public RunDelegate Run;
```

### **SelectContinue**

```csharp
public SelectContinueDelegate SelectContinue;
```

### **CanContinue**

```csharp
public CanContinueDelegate CanContinue;
```

### **IsWaiting**

```csharp
public IsWaitingDelegate IsWaiting;
```

### **SelectChoice**

```csharp
public SelectChoiceDelegate SelectChoice;
```

### **TryGetValue**

```csharp
public TryGetValueDelegate TryGetValue;
```

### **DestroyValue**

```csharp
public DestroyValueDelegate DestroyValue;
```

### **Subscribe**

```csharp
public SubscribeDelegate Subscribe;
```

### **Unsubscribe**

```csharp
public UnsubscribeDelegate Unsubscribe;
```

### **SetExternNumber**

```csharp
public SetExternNumberDelegate SetExternNumber;
```

### **SetExternString**

```csharp
public SetExternStringDelegate SetExternString;
```

### **SetExternBool**

```csharp
public SetExternBoolDelegate SetExternBool;
```

### **SetExternNil**

```csharp
public SetExternNilDelegate SetExternNil;
```

### **SetExternFunc**

```csharp
public SetExternFuncDelegate SetExternFunc;
```

### **CalculateStateSize**

```csharp
public CalculateStateSizeDelegate CalculateStateSize;
```

### **SaveState**

```csharp
public SaveStateDelegate SaveState;
```

### **LoadState**

```csharp
public LoadStateDelegate LoadState;
```

### **SetDebugLog**

```csharp
public SetDebugLogDelegate SetDebugLog;
```

### **SetDebugSeverity**

```csharp
public SetDebugSeverityDelegate SetDebugSeverity;
```

### **OnDebugLogMessage**

```csharp
public static Action<string, Severity> OnDebugLogMessage;
```

### **Loader**

Represents a loader for the PeartreeGames.Topiary library.

```csharp
public static ILoader Loader;
```

## Properties

### **Count**

Gets the count.

```csharp
public static int Count { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)  
The count.

### **Global**

Represents a global instance of the [Library](./peartreegames.topiary.library.md) class.

```csharp
public static Library Global { get; }
```

#### Property Value

[Library](./peartreegames.topiary.library.md)  

### **IsUnityRuntime**

Represents a property that indicates whether the program is running in Unity runtime or not.

```csharp
public static bool IsUnityRuntime { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  

### **Allocator**

Represents an allocator interface for memory management.

```csharp
public static IAllocator Allocator { get; set; }
```

#### Property Value

[IAllocator](./peartreegames.topiary.iallocator.md)  

## Constructors

### **Library()**

Represents a library that provides functionality for working with the Topiary dialogue system.

```csharp
public Library()
```

## Methods

### **Dispose()**

Releases all resources used by the object.

```csharp
public void Dispose()
```

### **PtrToUtf8String(IntPtr, Nullable&lt;Int32&gt;)**

Converts a pointer to a null-terminated UTF8-encoded string into a C# string.

```csharp
public static string PtrToUtf8String(IntPtr pointer, Nullable<int> count)
```

#### Parameters

`pointer` [IntPtr](https://docs.microsoft.com/en-us/dotnet/api/system.intptr)  
The pointer to the null-terminated UTF8-encoded string.

`count` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)  
Optional: The number of bytes to read from the pointer. Use null to read until null termination.

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The converted C# string.

**Remarks:**

Since we're targeting .net471 for unity we need to create our own ptr to utf8 it seems
