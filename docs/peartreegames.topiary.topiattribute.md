# TopiAttribute

Namespace: PeartreeGames.Topiary

Represents an attribute that declares a method as an extern topi function.
 Can only be used on static methods.

```csharp
public class TopiAttribute : System.Attribute
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.attribute) → [TopiAttribute](./peartreegames.topiary.topiattribute.md)

## Properties

### **Name**

Gets or sets the name of the function in the topi file.

```csharp
public string Name { get; private set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **TypeId**

```csharp
public object TypeId { get; }
```

#### Property Value

[Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)<br>

## Constructors

### **TopiAttribute(String)**

Declare the function as an extern topi function
 Can only be used on static methods

```csharp
public TopiAttribute(string name)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Name of the function in the topi file
