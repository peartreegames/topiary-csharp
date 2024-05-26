# ByteCode

Namespace: PeartreeGames.Topiary

Provides a set of methods for working with bytecode.

```csharp
public static class ByteCode
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ByteCode](./peartreegames.topiary.bytecode.md)

## Methods

### **GetExterns(BinaryReader)**

Retrieves a sorted set of external names from the given binary reader.

```csharp
public static SortedSet<string> GetExterns(BinaryReader reader)
```

#### Parameters

`reader` [BinaryReader](https://docs.microsoft.com/en-us/dotnet/api/system.io.binaryreader)  
The binary reader from which to read the external names.

#### Returns

SortedSet&lt;String&gt;  
A sorted set of external names.
