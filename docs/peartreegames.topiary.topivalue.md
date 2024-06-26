# TopiValue

Namespace: PeartreeGames.Topiary

Topiary Value container
 Data is overlapped in memory so ensure correct value is used
 or check tag if unknown

```csharp
public struct TopiValue
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [TopiValue](./peartreegames.topiary.topivalue.md)  
Implements [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable), [IEquatable&lt;TopiValue&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1)

## Fields

### **tag**

```csharp
public Tag tag;
```

## Properties

### **Bool**

```csharp
public bool Bool { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  

### **Int**

```csharp
public int Int { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)  

### **Float**

```csharp
public float Float { get; }
```

#### Property Value

[Single](https://docs.microsoft.com/en-us/dotnet/api/system.single)  

### **String**

```csharp
public string String { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  

### **List**

```csharp
public TopiValue[] List { get; }
```

#### Property Value

[TopiValue[]](./peartreegames.topiary.topivalue.md)  

### **Set**

```csharp
public HashSet<TopiValue> Set { get; }
```

#### Property Value

[HashSet&lt;TopiValue&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1)  

### **Map**

```csharp
public Dictionary<TopiValue, TopiValue> Map { get; }
```

#### Property Value

[Dictionary&lt;TopiValue, TopiValue&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2)  

### **Value**

```csharp
public object Value { get; }
```

#### Property Value

[Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)  

## Constructors

### **TopiValue(Boolean)**

```csharp
TopiValue(bool b)
```

#### Parameters

`b` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  

### **TopiValue(Int32)**

```csharp
TopiValue(int i)
```

#### Parameters

`i` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)  

### **TopiValue(Single)**

```csharp
TopiValue(float i)
```

#### Parameters

`i` [Single](https://docs.microsoft.com/en-us/dotnet/api/system.single)  

### **TopiValue(String)**

```csharp
TopiValue(string s)
```

#### Parameters

`s` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  

## Methods

### **FromPtr(IntPtr)**

Converts a pointer to a TopiValue struct.

```csharp
TopiValue FromPtr(IntPtr ptr)
```

#### Parameters

`ptr` [IntPtr](https://docs.microsoft.com/en-us/dotnet/api/system.intptr)  
The pointer to the TopiValue struct.

#### Returns

[TopiValue](./peartreegames.topiary.topivalue.md)  
The converted TopiValue.

### **AsType&lt;T&gt;()**

Gets the value of the TopiValue as the specified type.
 Will create boxing, better to use the above is value type is known

```csharp
T AsType<T>()
```

#### Type Parameters

`T`  
The type to convert the value to.

#### Returns

T  
The value of the TopiValue as type T.

**Remarks:**

This method converts the underlying value of the TopiValue to the specified type T.
 If the conversion is not possible, an InvalidCastException will be thrown.

### **ToString()**

Converts the TopiValue object to its string representation.

```csharp
string ToString()
```

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)  
The string representation of the TopiValue object.

### **Dispose()**

Releases the resources used by the TopiValue.

```csharp
void Dispose()
```

### **Equals(TopiValue)**

Determines whether the current instance is equal to the specified object.

```csharp
bool Equals(TopiValue other)
```

#### Parameters

`other` [TopiValue](./peartreegames.topiary.topivalue.md)  
The object to compare with the current instance.

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  
True if the current instance is equal to the specified object; otherwise, false.

### **Equals(Object)**

Determines whether the current instance is equal to another TopiValue object.

```csharp
bool Equals(object obj)
```

#### Parameters

`obj` [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)  
The TopiValue object to compare with the current instance.

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)  
true if the current instance is equal to the other object; otherwise, false.

### **GetHashCode()**

Gets the hash code of the TopiValue object.

```csharp
int GetHashCode()
```

#### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)  
The hash code of the TopiValue object.
