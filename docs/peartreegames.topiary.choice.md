# Choice

Namespace: PeartreeGames.Topiary

Represents a choice in a dialogue.

```csharp
public struct Choice
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [Choice](./peartreegames.topiary.choice.md)

## Properties

### **VisitCount**

Gets the visit count associated with the choice.

```csharp
public int VisitCount { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)  
The visit count.

### **Ip**

Gets the IP (Instruction Pointer) of the choice.

```csharp
public int Ip { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)  

**Remarks:**

Mostly used internally, but exposed here as well

### **Content**

Represents a choice in a dialogue.

```csharp
public string Content { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  

### **Tags**

Gets the tags associated with the choice.

```csharp
public String[] Tags { get; }
```

#### Property Value

[String[]](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The tags associated with the choice.

**Remarks:**

The tags are represented as an array of strings.

## Methods

### **MarshalPtr(IntPtr, Byte)**

Marshals an [IntPtr](https://docs.microsoft.com/en-us/dotnet/api/system.intptr) pointer to an array of [Choice](./peartreegames.topiary.choice.md) structures.

```csharp
Choice[] MarshalPtr(IntPtr choicePtr, byte count)
```

#### Parameters

`choicePtr` [IntPtr](https://docs.microsoft.com/en-us/dotnet/api/system.intptr)  
The pointer to the array of [Choice](./peartreegames.topiary.choice.md) structures.

`count` [Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)  
The number of [Choice](./peartreegames.topiary.choice.md) structures in the array.

#### Returns

[Choice[]](./peartreegames.topiary.choice.md)  
An array of [Choice](./peartreegames.topiary.choice.md) structures.
