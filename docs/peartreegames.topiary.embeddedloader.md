# EmbeddedLoader

Namespace: PeartreeGames.Topiary

Represents a loader interface for loading and interacting with libraries.

```csharp
public static class EmbeddedLoader
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [EmbeddedLoader](./peartreegames.topiary.embeddedloader.md)

## Methods

### **CreateEmbeddedResource(String)**

Creates an embedded resource from the specified DLL name.

```csharp
public static string CreateEmbeddedResource(string dllName)
```

#### Parameters

`dllName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The name of the DLL.

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The temporary file path where the embedded resource is created.
