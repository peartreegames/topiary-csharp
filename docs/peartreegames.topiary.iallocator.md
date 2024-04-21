# IAllocator

Namespace: PeartreeGames.Topiary

```csharp
public interface IAllocator
```

## Methods

### **Allocate(UInt64)**

```csharp
IntPtr Allocate(ulong size)
```

#### Parameters

`size` [UInt64](https://docs.microsoft.com/en-us/dotnet/api/system.uint64)  

#### Returns

[IntPtr](https://docs.microsoft.com/en-us/dotnet/api/system.intptr)  

### **Deallocate(IntPtr)**

```csharp
void Deallocate(IntPtr ptr)
```

#### Parameters

`ptr` [IntPtr](https://docs.microsoft.com/en-us/dotnet/api/system.intptr)  
