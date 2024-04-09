# ManagedAllocator

Namespace: PeartreeGames.Topiary

```csharp
public struct ManagedAllocator
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [ManagedAllocator](./peartreegames.topiary.managedallocator.md)<br>
Implements [IAllocator](./peartreegames.topiary.iallocator.md)

## Fields

### **Global**

```csharp
public static ManagedAllocator Global;
```

## Methods

### **Allocate(UInt64)**

```csharp
IntPtr Allocate(ulong size)
```

#### Parameters

`size` [UInt64](https://docs.microsoft.com/en-us/dotnet/api/system.uint64)<br>

#### Returns

[IntPtr](https://docs.microsoft.com/en-us/dotnet/api/system.intptr)<br>

### **Deallocate(IntPtr)**

```csharp
void Deallocate(IntPtr ptr)
```

#### Parameters

`ptr` [IntPtr](https://docs.microsoft.com/en-us/dotnet/api/system.intptr)<br>
