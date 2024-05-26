# Topiary.CSharp

## Contents

- [ByteCode](#T-PeartreeGames-Topiary-ByteCode 'PeartreeGames.Topiary.ByteCode')
  - [GetExterns(reader)](#M-PeartreeGames-Topiary-ByteCode-GetExterns-System-IO-BinaryReader- 'PeartreeGames.Topiary.ByteCode.GetExterns(System.IO.BinaryReader)')
- [CalculateCompileSizeDelegate](#T-PeartreeGames-Topiary-Delegates-CalculateCompileSizeDelegate 'PeartreeGames.Topiary.Delegates.CalculateCompileSizeDelegate')
- [CalculateStateSizeDelegate](#T-PeartreeGames-Topiary-Delegates-CalculateStateSizeDelegate 'PeartreeGames.Topiary.Delegates.CalculateStateSizeDelegate')
- [CanContinueDelegate](#T-PeartreeGames-Topiary-Delegates-CanContinueDelegate 'PeartreeGames.Topiary.Delegates.CanContinueDelegate')
- [Choice](#T-PeartreeGames-Topiary-Choice 'PeartreeGames.Topiary.Choice')
  - [Content](#P-PeartreeGames-Topiary-Choice-Content 'PeartreeGames.Topiary.Choice.Content')
  - [Ip](#P-PeartreeGames-Topiary-Choice-Ip 'PeartreeGames.Topiary.Choice.Ip')
  - [Tags](#P-PeartreeGames-Topiary-Choice-Tags 'PeartreeGames.Topiary.Choice.Tags')
  - [VisitCount](#P-PeartreeGames-Topiary-Choice-VisitCount 'PeartreeGames.Topiary.Choice.VisitCount')
  - [MarshalPtr(choicePtr,count)](#M-PeartreeGames-Topiary-Choice-MarshalPtr-System-IntPtr,System-Byte- 'PeartreeGames.Topiary.Choice.MarshalPtr(System.IntPtr,System.Byte)')
- [CompileDelegate](#T-PeartreeGames-Topiary-Delegates-CompileDelegate 'PeartreeGames.Topiary.Delegates.CompileDelegate')
- [CreateVmDelegate](#T-PeartreeGames-Topiary-Delegates-CreateVmDelegate 'PeartreeGames.Topiary.Delegates.CreateVmDelegate')
- [Delegates](#T-PeartreeGames-Topiary-Delegates 'PeartreeGames.Topiary.Delegates')
- [DestroyValueDelegate](#T-PeartreeGames-Topiary-Delegates-DestroyValueDelegate 'PeartreeGames.Topiary.Delegates.DestroyValueDelegate')
- [DestroyVmDelegate](#T-PeartreeGames-Topiary-Delegates-DestroyVmDelegate 'PeartreeGames.Topiary.Delegates.DestroyVmDelegate')
- [Dialogue](#T-PeartreeGames-Topiary-Dialogue 'PeartreeGames.Topiary.Dialogue')
  - [#ctor()](#M-PeartreeGames-Topiary-Dialogue-#ctor-System-Byte[],PeartreeGames-Topiary-Dialogue-OnLineCallback,PeartreeGames-Topiary-Dialogue-OnChoicesCallback,PeartreeGames-Topiary-Library-Severity- 'PeartreeGames.Topiary.Dialogue.#ctor(System.Byte[],PeartreeGames.Topiary.Dialogue.OnLineCallback,PeartreeGames.Topiary.Dialogue.OnChoicesCallback,PeartreeGames.Topiary.Library.Severity)')
  - [CanContinue](#P-PeartreeGames-Topiary-Dialogue-CanContinue 'PeartreeGames.Topiary.Dialogue.CanContinue')
  - [IsValid](#P-PeartreeGames-Topiary-Dialogue-IsValid 'PeartreeGames.Topiary.Dialogue.IsValid')
  - [IsWaiting](#P-PeartreeGames-Topiary-Dialogue-IsWaiting 'PeartreeGames.Topiary.Dialogue.IsWaiting')
  - [Library](#P-PeartreeGames-Topiary-Dialogue-Library 'PeartreeGames.Topiary.Dialogue.Library')
  - [BindFunctions(assemblies)](#M-PeartreeGames-Topiary-Dialogue-BindFunctions-System-Collections-Generic-IEnumerable{System-Reflection-Assembly}- 'PeartreeGames.Topiary.Dialogue.BindFunctions(System.Collections.Generic.IEnumerable{System.Reflection.Assembly})')
  - [Compile(fullPath,severity)](#M-PeartreeGames-Topiary-Dialogue-Compile-System-String,PeartreeGames-Topiary-Library-Severity- 'PeartreeGames.Topiary.Dialogue.Compile(System.String,PeartreeGames.Topiary.Library.Severity)')
  - [Compile(fullPath,capacity,severity)](#M-PeartreeGames-Topiary-Dialogue-Compile-System-String,System-Int64,PeartreeGames-Topiary-Library-Severity- 'PeartreeGames.Topiary.Dialogue.Compile(System.String,System.Int64,PeartreeGames.Topiary.Library.Severity)')
  - [Continue()](#M-PeartreeGames-Topiary-Dialogue-Continue 'PeartreeGames.Topiary.Dialogue.Continue')
  - [DestroyValue(value)](#M-PeartreeGames-Topiary-Dialogue-DestroyValue-PeartreeGames-Topiary-TopiValue@- 'PeartreeGames.Topiary.Dialogue.DestroyValue(PeartreeGames.Topiary.TopiValue@)')
  - [Dispose()](#M-PeartreeGames-Topiary-Dialogue-Dispose 'PeartreeGames.Topiary.Dialogue.Dispose')
  - [GetValue(name)](#M-PeartreeGames-Topiary-Dialogue-GetValue-System-String- 'PeartreeGames.Topiary.Dialogue.GetValue(System.String)')
  - [LoadState(json)](#M-PeartreeGames-Topiary-Dialogue-LoadState-System-String- 'PeartreeGames.Topiary.Dialogue.LoadState(System.String)')
  - [Run()](#M-PeartreeGames-Topiary-Dialogue-Run 'PeartreeGames.Topiary.Dialogue.Run')
  - [SaveState()](#M-PeartreeGames-Topiary-Dialogue-SaveState 'PeartreeGames.Topiary.Dialogue.SaveState')
  - [SaveState(capacity)](#M-PeartreeGames-Topiary-Dialogue-SaveState-System-Int64- 'PeartreeGames.Topiary.Dialogue.SaveState(System.Int64)')
  - [SelectChoice(index)](#M-PeartreeGames-Topiary-Dialogue-SelectChoice-System-Int32- 'PeartreeGames.Topiary.Dialogue.SelectChoice(System.Int32)')
  - [Set(name,value)](#M-PeartreeGames-Topiary-Dialogue-Set-System-String,System-Boolean- 'PeartreeGames.Topiary.Dialogue.Set(System.String,System.Boolean)')
  - [Set(name,value)](#M-PeartreeGames-Topiary-Dialogue-Set-System-String,System-Single- 'PeartreeGames.Topiary.Dialogue.Set(System.String,System.Single)')
  - [Set(name,value)](#M-PeartreeGames-Topiary-Dialogue-Set-System-String,System-String- 'PeartreeGames.Topiary.Dialogue.Set(System.String,System.String)')
  - [Set(name,function,arity)](#M-PeartreeGames-Topiary-Dialogue-Set-System-String,PeartreeGames-Topiary-Function,System-Byte- 'PeartreeGames.Topiary.Dialogue.Set(System.String,PeartreeGames.Topiary.Function,System.Byte)')
  - [Start(bough)](#M-PeartreeGames-Topiary-Dialogue-Start-System-String- 'PeartreeGames.Topiary.Dialogue.Start(System.String)')
  - [Subscribe(name,callback)](#M-PeartreeGames-Topiary-Dialogue-Subscribe-System-String,PeartreeGames-Topiary-Delegates-Subscriber- 'PeartreeGames.Topiary.Dialogue.Subscribe(System.String,PeartreeGames.Topiary.Delegates.Subscriber)')
  - [Unset(name)](#M-PeartreeGames-Topiary-Dialogue-Unset-System-String- 'PeartreeGames.Topiary.Dialogue.Unset(System.String)')
  - [Unsubscribe(name,callback)](#M-PeartreeGames-Topiary-Dialogue-Unsubscribe-System-String,PeartreeGames-Topiary-Delegates-Subscriber- 'PeartreeGames.Topiary.Dialogue.Unsubscribe(System.String,PeartreeGames.Topiary.Delegates.Subscriber)')
- [EmbeddedLoader](#T-PeartreeGames-Topiary-EmbeddedLoader 'PeartreeGames.Topiary.EmbeddedLoader')
  - [CreateEmbeddedResource(dllName)](#M-PeartreeGames-Topiary-EmbeddedLoader-CreateEmbeddedResource-System-String- 'PeartreeGames.Topiary.EmbeddedLoader.CreateEmbeddedResource(System.String)')
- [Function](#T-PeartreeGames-Topiary-Function 'PeartreeGames.Topiary.Function')
  - [Call(argPtr,count)](#M-PeartreeGames-Topiary-Function-Call-System-IntPtr,System-Byte- 'PeartreeGames.Topiary.Function.Call(System.IntPtr,System.Byte)')
  - [Create(method)](#M-PeartreeGames-Topiary-Function-Create-System-Reflection-MethodInfo- 'PeartreeGames.Topiary.Function.Create(System.Reflection.MethodInfo)')
  - [CreateArgs(argPtr,count)](#M-PeartreeGames-Topiary-Function-CreateArgs-System-IntPtr,System-Byte- 'PeartreeGames.Topiary.Function.CreateArgs(System.IntPtr,System.Byte)')
  - [Dispose()](#M-PeartreeGames-Topiary-Function-Dispose 'PeartreeGames.Topiary.Function.Dispose')
  - [GetCallIntPtr()](#M-PeartreeGames-Topiary-Function-GetCallIntPtr 'PeartreeGames.Topiary.Function.GetCallIntPtr')
  - [ToString()](#M-PeartreeGames-Topiary-Function-ToString 'PeartreeGames.Topiary.Function.ToString')
- [ILoader](#T-PeartreeGames-Topiary-ILoader 'PeartreeGames.Topiary.ILoader')
  - [Free(ptr)](#M-PeartreeGames-Topiary-ILoader-Free-System-IntPtr- 'PeartreeGames.Topiary.ILoader.Free(System.IntPtr)')
  - [GetProc(name)](#M-PeartreeGames-Topiary-ILoader-GetProc-System-String- 'PeartreeGames.Topiary.ILoader.GetProc(System.String)')
  - [Load()](#M-PeartreeGames-Topiary-ILoader-Load 'PeartreeGames.Topiary.ILoader.Load')
- [IsWaitingDelegate](#T-PeartreeGames-Topiary-Delegates-IsWaitingDelegate 'PeartreeGames.Topiary.Delegates.IsWaitingDelegate')
- [Library](#T-PeartreeGames-Topiary-Library 'PeartreeGames.Topiary.Library')
  - [#ctor()](#M-PeartreeGames-Topiary-Library-#ctor 'PeartreeGames.Topiary.Library.#ctor')
  - [Loader](#F-PeartreeGames-Topiary-Library-Loader 'PeartreeGames.Topiary.Library.Loader')
  - [OnDebugLogMessage](#F-PeartreeGames-Topiary-Library-OnDebugLogMessage 'PeartreeGames.Topiary.Library.OnDebugLogMessage')
  - [Allocator](#P-PeartreeGames-Topiary-Library-Allocator 'PeartreeGames.Topiary.Library.Allocator')
  - [Count](#P-PeartreeGames-Topiary-Library-Count 'PeartreeGames.Topiary.Library.Count')
  - [Global](#P-PeartreeGames-Topiary-Library-Global 'PeartreeGames.Topiary.Library.Global')
  - [IsUnityRuntime](#P-PeartreeGames-Topiary-Library-IsUnityRuntime 'PeartreeGames.Topiary.Library.IsUnityRuntime')
  - [Dispose()](#M-PeartreeGames-Topiary-Library-Dispose 'PeartreeGames.Topiary.Library.Dispose')
  - [PtrToUtf8String(pointer,count)](#M-PeartreeGames-Topiary-Library-PtrToUtf8String-System-IntPtr,System-Nullable{System-Int32}- 'PeartreeGames.Topiary.Library.PtrToUtf8String(System.IntPtr,System.Nullable{System.Int32})')
- [Line](#T-PeartreeGames-Topiary-Line 'PeartreeGames.Topiary.Line')
  - [Content](#P-PeartreeGames-Topiary-Line-Content 'PeartreeGames.Topiary.Line.Content')
  - [Speaker](#P-PeartreeGames-Topiary-Line-Speaker 'PeartreeGames.Topiary.Line.Speaker')
  - [Tags](#P-PeartreeGames-Topiary-Line-Tags 'PeartreeGames.Topiary.Line.Tags')
- [LoadStateDelegate](#T-PeartreeGames-Topiary-Delegates-LoadStateDelegate 'PeartreeGames.Topiary.Delegates.LoadStateDelegate')
- [MacLoader](#T-PeartreeGames-Topiary-MacLoader 'PeartreeGames.Topiary.MacLoader')
  - [#ctor()](#M-PeartreeGames-Topiary-MacLoader-#ctor-System-Boolean- 'PeartreeGames.Topiary.MacLoader.#ctor(System.Boolean)')
  - [Free(ptr)](#M-PeartreeGames-Topiary-MacLoader-Free-System-IntPtr- 'PeartreeGames.Topiary.MacLoader.Free(System.IntPtr)')
  - [GetProc(name)](#M-PeartreeGames-Topiary-MacLoader-GetProc-System-String- 'PeartreeGames.Topiary.MacLoader.GetProc(System.String)')
  - [Load()](#M-PeartreeGames-Topiary-MacLoader-Load 'PeartreeGames.Topiary.MacLoader.Load')
  - [ReleaseHandle()](#M-PeartreeGames-Topiary-MacLoader-ReleaseHandle 'PeartreeGames.Topiary.MacLoader.ReleaseHandle')
- [OnChoicesCallback](#T-PeartreeGames-Topiary-Dialogue-OnChoicesCallback 'PeartreeGames.Topiary.Dialogue.OnChoicesCallback')
- [OnChoicesDelegate](#T-PeartreeGames-Topiary-Delegates-OnChoicesDelegate 'PeartreeGames.Topiary.Delegates.OnChoicesDelegate')
- [OnLineCallback](#T-PeartreeGames-Topiary-Dialogue-OnLineCallback 'PeartreeGames.Topiary.Dialogue.OnLineCallback')
- [OnLineDelegate](#T-PeartreeGames-Topiary-Delegates-OnLineDelegate 'PeartreeGames.Topiary.Delegates.OnLineDelegate')
- [RunDelegate](#T-PeartreeGames-Topiary-Delegates-RunDelegate 'PeartreeGames.Topiary.Delegates.RunDelegate')
- [SaveStateDelegate](#T-PeartreeGames-Topiary-Delegates-SaveStateDelegate 'PeartreeGames.Topiary.Delegates.SaveStateDelegate')
- [SelectChoiceDelegate](#T-PeartreeGames-Topiary-Delegates-SelectChoiceDelegate 'PeartreeGames.Topiary.Delegates.SelectChoiceDelegate')
- [SelectContinueDelegate](#T-PeartreeGames-Topiary-Delegates-SelectContinueDelegate 'PeartreeGames.Topiary.Delegates.SelectContinueDelegate')
- [SetDebugLogDelegate](#T-PeartreeGames-Topiary-Delegates-SetDebugLogDelegate 'PeartreeGames.Topiary.Delegates.SetDebugLogDelegate')
- [SetDebugSeverityDelegate](#T-PeartreeGames-Topiary-Delegates-SetDebugSeverityDelegate 'PeartreeGames.Topiary.Delegates.SetDebugSeverityDelegate')
- [SetExternBoolDelegate](#T-PeartreeGames-Topiary-Delegates-SetExternBoolDelegate 'PeartreeGames.Topiary.Delegates.SetExternBoolDelegate')
- [SetExternFuncDelegate](#T-PeartreeGames-Topiary-Delegates-SetExternFuncDelegate 'PeartreeGames.Topiary.Delegates.SetExternFuncDelegate')
- [SetExternNilDelegate](#T-PeartreeGames-Topiary-Delegates-SetExternNilDelegate 'PeartreeGames.Topiary.Delegates.SetExternNilDelegate')
- [SetExternNumberDelegate](#T-PeartreeGames-Topiary-Delegates-SetExternNumberDelegate 'PeartreeGames.Topiary.Delegates.SetExternNumberDelegate')
- [SetExternStringDelegate](#T-PeartreeGames-Topiary-Delegates-SetExternStringDelegate 'PeartreeGames.Topiary.Delegates.SetExternStringDelegate')
- [Severity](#T-PeartreeGames-Topiary-Library-Severity 'PeartreeGames.Topiary.Library.Severity')
- [StartDelegate](#T-PeartreeGames-Topiary-Delegates-StartDelegate 'PeartreeGames.Topiary.Delegates.StartDelegate')
- [SubscribeDelegate](#T-PeartreeGames-Topiary-Delegates-SubscribeDelegate 'PeartreeGames.Topiary.Delegates.SubscribeDelegate')
- [Subscriber](#T-PeartreeGames-Topiary-Delegates-Subscriber 'PeartreeGames.Topiary.Delegates.Subscriber')
- [Tag](#T-PeartreeGames-Topiary-TopiValue-Tag 'PeartreeGames.Topiary.TopiValue.Tag')
  - [Bool](#F-PeartreeGames-Topiary-TopiValue-Tag-Bool 'PeartreeGames.Topiary.TopiValue.Tag.Bool')
  - [List](#F-PeartreeGames-Topiary-TopiValue-Tag-List 'PeartreeGames.Topiary.TopiValue.Tag.List')
  - [Map](#F-PeartreeGames-Topiary-TopiValue-Tag-Map 'PeartreeGames.Topiary.TopiValue.Tag.Map')
  - [Nil](#F-PeartreeGames-Topiary-TopiValue-Tag-Nil 'PeartreeGames.Topiary.TopiValue.Tag.Nil')
  - [Number](#F-PeartreeGames-Topiary-TopiValue-Tag-Number 'PeartreeGames.Topiary.TopiValue.Tag.Number')
  - [Set](#F-PeartreeGames-Topiary-TopiValue-Tag-Set 'PeartreeGames.Topiary.TopiValue.Tag.Set')
  - [String](#F-PeartreeGames-Topiary-TopiValue-Tag-String 'PeartreeGames.Topiary.TopiValue.Tag.String')
- [TopiAttribute](#T-PeartreeGames-Topiary-TopiAttribute 'PeartreeGames.Topiary.TopiAttribute')
  - [#ctor(name)](#M-PeartreeGames-Topiary-TopiAttribute-#ctor-System-String- 'PeartreeGames.Topiary.TopiAttribute.#ctor(System.String)')
  - [Name](#P-PeartreeGames-Topiary-TopiAttribute-Name 'PeartreeGames.Topiary.TopiAttribute.Name')
- [TopiValue](#T-PeartreeGames-Topiary-TopiValue 'PeartreeGames.Topiary.TopiValue')
  - [AsType\`\`1()](#M-PeartreeGames-Topiary-TopiValue-AsType``1 'PeartreeGames.Topiary.TopiValue.AsType``1')
  - [Dispose()](#M-PeartreeGames-Topiary-TopiValue-Dispose 'PeartreeGames.Topiary.TopiValue.Dispose')
  - [Equals(other)](#M-PeartreeGames-Topiary-TopiValue-Equals-PeartreeGames-Topiary-TopiValue- 'PeartreeGames.Topiary.TopiValue.Equals(PeartreeGames.Topiary.TopiValue)')
  - [Equals(obj)](#M-PeartreeGames-Topiary-TopiValue-Equals-System-Object- 'PeartreeGames.Topiary.TopiValue.Equals(System.Object)')
  - [FromPtr(ptr)](#M-PeartreeGames-Topiary-TopiValue-FromPtr-System-IntPtr- 'PeartreeGames.Topiary.TopiValue.FromPtr(System.IntPtr)')
  - [GetHashCode()](#M-PeartreeGames-Topiary-TopiValue-GetHashCode 'PeartreeGames.Topiary.TopiValue.GetHashCode')
  - [ToString()](#M-PeartreeGames-Topiary-TopiValue-ToString 'PeartreeGames.Topiary.TopiValue.ToString')
- [TryGetValueDelegate](#T-PeartreeGames-Topiary-Delegates-TryGetValueDelegate 'PeartreeGames.Topiary.Delegates.TryGetValueDelegate')
- [UnsubscribeDelegate](#T-PeartreeGames-Topiary-Delegates-UnsubscribeDelegate 'PeartreeGames.Topiary.Delegates.UnsubscribeDelegate')
- [WindowsLoader](#T-PeartreeGames-Topiary-WindowsLoader 'PeartreeGames.Topiary.WindowsLoader')
  - [#ctor()](#M-PeartreeGames-Topiary-WindowsLoader-#ctor-System-Boolean- 'PeartreeGames.Topiary.WindowsLoader.#ctor(System.Boolean)')
  - [Free(ptr)](#M-PeartreeGames-Topiary-WindowsLoader-Free-System-IntPtr- 'PeartreeGames.Topiary.WindowsLoader.Free(System.IntPtr)')
  - [GetProc(name)](#M-PeartreeGames-Topiary-WindowsLoader-GetProc-System-String- 'PeartreeGames.Topiary.WindowsLoader.GetProc(System.String)')
  - [Load()](#M-PeartreeGames-Topiary-WindowsLoader-Load 'PeartreeGames.Topiary.WindowsLoader.Load')
  - [ReleaseHandle()](#M-PeartreeGames-Topiary-WindowsLoader-ReleaseHandle 'PeartreeGames.Topiary.WindowsLoader.ReleaseHandle')

<a name='T-PeartreeGames-Topiary-ByteCode'></a>
## ByteCode `type`

##### Namespace

PeartreeGames.Topiary

##### Summary

Provides a set of methods for working with bytecode.

<a name='M-PeartreeGames-Topiary-ByteCode-GetExterns-System-IO-BinaryReader-'></a>
### GetExterns(reader) `method`

##### Summary

Retrieves a sorted set of external names from the given binary reader.

##### Returns

A sorted set of external names.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| reader | [System.IO.BinaryReader](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IO.BinaryReader 'System.IO.BinaryReader') | The binary reader from which to read the external names. |

<a name='T-PeartreeGames-Topiary-Delegates-CalculateCompileSizeDelegate'></a>
## CalculateCompileSizeDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Delegate for calculating the compile size.

##### Returns

The calculated compile size.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [T:PeartreeGames.Topiary.Delegates.CalculateCompileSizeDelegate](#T-T-PeartreeGames-Topiary-Delegates-CalculateCompileSizeDelegate 'T:PeartreeGames.Topiary.Delegates.CalculateCompileSizeDelegate') | The path of the file to calculate the compile size. |

<a name='T-PeartreeGames-Topiary-Delegates-CalculateStateSizeDelegate'></a>
## CalculateStateSizeDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Represents a delegate used to calculate the size of the state.

##### Returns

The size of the state.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.CalculateStateSizeDelegate](#T-T-PeartreeGames-Topiary-Delegates-CalculateStateSizeDelegate 'T:PeartreeGames.Topiary.Delegates.CalculateStateSizeDelegate') | A pointer to the virtual machine. |

<a name='T-PeartreeGames-Topiary-Delegates-CanContinueDelegate'></a>
## CanContinueDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Represents a delegate used to determine if a library can continue.

##### Returns

`true` if the library can continue; otherwise, `false`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.CanContinueDelegate](#T-T-PeartreeGames-Topiary-Delegates-CanContinueDelegate 'T:PeartreeGames.Topiary.Delegates.CanContinueDelegate') | The pointer to the virtual machine. |

<a name='T-PeartreeGames-Topiary-Choice'></a>
## Choice `type`

##### Namespace

PeartreeGames.Topiary

##### Summary

Represents a choice in a dialogue.

<a name='P-PeartreeGames-Topiary-Choice-Content'></a>
### Content `property`

##### Summary

Represents a choice in a dialogue.

<a name='P-PeartreeGames-Topiary-Choice-Ip'></a>
### Ip `property`

##### Summary

Gets the IP (Instruction Pointer) of the choice.

##### Remarks

Mostly used internally, but exposed here as well

<a name='P-PeartreeGames-Topiary-Choice-Tags'></a>
### Tags `property`

##### Summary

Gets the tags associated with the choice.

##### Remarks

The tags are represented as an array of strings.

<a name='P-PeartreeGames-Topiary-Choice-VisitCount'></a>
### VisitCount `property`

##### Summary

Gets the visit count associated with the choice.

<a name='M-PeartreeGames-Topiary-Choice-MarshalPtr-System-IntPtr,System-Byte-'></a>
### MarshalPtr(choicePtr,count) `method`

##### Summary

Marshals an [IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') pointer to an array of [Choice](#T-PeartreeGames-Topiary-Choice 'PeartreeGames.Topiary.Choice') structures.

##### Returns

An array of [Choice](#T-PeartreeGames-Topiary-Choice 'PeartreeGames.Topiary.Choice') structures.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| choicePtr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The pointer to the array of [Choice](#T-PeartreeGames-Topiary-Choice 'PeartreeGames.Topiary.Choice') structures. |
| count | [System.Byte](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte 'System.Byte') | The number of [Choice](#T-PeartreeGames-Topiary-Choice 'PeartreeGames.Topiary.Choice') structures in the array. |

<a name='T-PeartreeGames-Topiary-Delegates-CompileDelegate'></a>
## CompileDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Represents a delegate for compiling a specified path into a byte array output.

##### Returns

An integer value indicating the result of the compilation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [T:PeartreeGames.Topiary.Delegates.CompileDelegate](#T-T-PeartreeGames-Topiary-Delegates-CompileDelegate 'T:PeartreeGames.Topiary.Delegates.CompileDelegate') | The path to compile. |

<a name='T-PeartreeGames-Topiary-Delegates-CreateVmDelegate'></a>
## CreateVmDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Delegate for creating a virtual machine.

##### Returns

A pointer to the created virtual machine.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| source | [T:PeartreeGames.Topiary.Delegates.CreateVmDelegate](#T-T-PeartreeGames-Topiary-Delegates-CreateVmDelegate 'T:PeartreeGames.Topiary.Delegates.CreateVmDelegate') | The source code as a byte array. |

<a name='T-PeartreeGames-Topiary-Delegates'></a>
## Delegates `type`

##### Namespace

PeartreeGames.Topiary

##### Summary

The Delegates class provides delegates for various functions used in the PeartreeGames.Topiary namespace.

##### Remarks

These delegates are used for callback functions, function pointers, and event handlers within the Topiary library.

<a name='T-PeartreeGames-Topiary-Delegates-DestroyValueDelegate'></a>
## DestroyValueDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

<a name='T-PeartreeGames-Topiary-Delegates-DestroyVmDelegate'></a>
## DestroyVmDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Delegate representing the DestroyVm method.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.DestroyVmDelegate](#T-T-PeartreeGames-Topiary-Delegates-DestroyVmDelegate 'T:PeartreeGames.Topiary.Delegates.DestroyVmDelegate') | Pointer to the virtual machine. |

<a name='T-PeartreeGames-Topiary-Dialogue'></a>
## Dialogue `type`

##### Namespace

PeartreeGames.Topiary

##### Summary

Represents a dialogue instance.

<a name='M-PeartreeGames-Topiary-Dialogue-#ctor-System-Byte[],PeartreeGames-Topiary-Dialogue-OnLineCallback,PeartreeGames-Topiary-Dialogue-OnChoicesCallback,PeartreeGames-Topiary-Library-Severity-'></a>
### #ctor() `constructor`

##### Summary

Represents a dialogue.

##### Parameters

This constructor has no parameters.

<a name='P-PeartreeGames-Topiary-Dialogue-CanContinue'></a>
### CanContinue `property`

##### Summary

Gets a value indicating whether the dialogue can continue.

<a name='P-PeartreeGames-Topiary-Dialogue-IsValid'></a>
### IsValid `property`

##### Summary

Gets a value indicating whether the Dialogue instance is valid.

##### Remarks

This property returns `true` if the internal pointer `_vmPtr` is not zero;
otherwise, it returns `false`.

<a name='P-PeartreeGames-Topiary-Dialogue-IsWaiting'></a>
### IsWaiting `property`

##### Summary

Gets a value indicating whether the dialogue is waiting for user input.

<a name='P-PeartreeGames-Topiary-Dialogue-Library'></a>
### Library `property`

##### Summary

Represents a library that provides functionality for creating and managing dialogues.

<a name='M-PeartreeGames-Topiary-Dialogue-BindFunctions-System-Collections-Generic-IEnumerable{System-Reflection-Assembly}-'></a>
### BindFunctions(assemblies) `method`

##### Summary

Bind all TopiAttribute functions within the given Assemblies
Functions must be of type "Func<TopiValue[,TopiValue,TopiValue,TopiValue,TopiValue]>"
or "Action[<TopiValue,TopiValue,TopiValue,TopiValue>]"
See [Function](#T-PeartreeGames-Topiary-Function 'PeartreeGames.Topiary.Function')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| assemblies | [System.Collections.Generic.IEnumerable{System.Reflection.Assembly}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{System.Reflection.Assembly}') |  |

<a name='M-PeartreeGames-Topiary-Dialogue-Compile-System-String,PeartreeGames-Topiary-Library-Severity-'></a>
### Compile(fullPath,severity) `method`

##### Summary

Compile a ".topi" file into bytes.
Should be saved to a ".topib" file
Will precalculate the required capacity

##### Returns

Compiled bytes

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fullPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The file absolute path |
| severity | [PeartreeGames.Topiary.Library.Severity](#T-PeartreeGames-Topiary-Library-Severity 'PeartreeGames.Topiary.Library.Severity') | Log severity |

<a name='M-PeartreeGames-Topiary-Dialogue-Compile-System-String,System-Int64,PeartreeGames-Topiary-Library-Severity-'></a>
### Compile(fullPath,capacity,severity) `method`

##### Summary

Compile a ".topi" file into bytes.
Should be saved to a ".topib" file

##### Returns

Compiled bytes

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fullPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The file absolute path |
| capacity | [System.Int64](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int64 'System.Int64') | Capacity for the bytecode output |
| severity | [PeartreeGames.Topiary.Library.Severity](#T-PeartreeGames-Topiary-Library-Severity 'PeartreeGames.Topiary.Library.Severity') | Log severity |

<a name='M-PeartreeGames-Topiary-Dialogue-Continue'></a>
### Continue() `method`

##### Summary

Continue the Dialogue.

##### Parameters

This method has no parameters.

<a name='M-PeartreeGames-Topiary-Dialogue-DestroyValue-PeartreeGames-Topiary-TopiValue@-'></a>
### DestroyValue(value) `method`

##### Summary

Destroy a reference value in unmanaged memory
NOTE: This should be removed in future versions

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [PeartreeGames.Topiary.TopiValue@](#T-PeartreeGames-Topiary-TopiValue@ 'PeartreeGames.Topiary.TopiValue@') | The value to be destroyed |

<a name='M-PeartreeGames-Topiary-Dialogue-Dispose'></a>
### Dispose() `method`

##### Summary

Releases all resources used by the Dialogue object.

##### Parameters

This method has no parameters.

<a name='M-PeartreeGames-Topiary-Dialogue-GetValue-System-String-'></a>
### GetValue(name) `method`

##### Summary

Retrieve the current value of any Global variable in the dialogue

##### Returns

TopiValue.nil if not found

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the variable |

<a name='M-PeartreeGames-Topiary-Dialogue-LoadState-System-String-'></a>
### LoadState(json) `method`

##### Summary

Load a JSON state into the Dialogue

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| json | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | JSON string |

<a name='M-PeartreeGames-Topiary-Dialogue-Run'></a>
### Run() `method`

##### Summary

Run the Dialogue until the next Dialogue or Choice

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') |  |

<a name='M-PeartreeGames-Topiary-Dialogue-SaveState'></a>
### SaveState() `method`

##### Summary

Save current Dialogue State to JSON
Will precalculate the necessary size
Should merge the resulting JSON with the Game Root JSON State

##### Parameters

This method has no parameters.

<a name='M-PeartreeGames-Topiary-Dialogue-SaveState-System-Int64-'></a>
### SaveState(capacity) `method`

##### Summary

Save current Dialogue State to JSON
Should merge the resulting JSON with the Game Root JSON State

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| capacity | [System.Int64](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int64 'System.Int64') | The maximum size of bytes to allocate |

<a name='M-PeartreeGames-Topiary-Dialogue-SelectChoice-System-Int32-'></a>
### SelectChoice(index) `method`

##### Summary

Select a Choice.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| index | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The index of the choice selected |

<a name='M-PeartreeGames-Topiary-Dialogue-Set-System-String,System-Boolean-'></a>
### Set(name,value) `method`

##### Summary

Set an Extern variable to a bool value

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the variable |
| value | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | The value to set |

<a name='M-PeartreeGames-Topiary-Dialogue-Set-System-String,System-Single-'></a>
### Set(name,value) `method`

##### Summary

Set an Extern variable to a float value

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the variable |
| value | [System.Single](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Single 'System.Single') | The value to set |

<a name='M-PeartreeGames-Topiary-Dialogue-Set-System-String,System-String-'></a>
### Set(name,value) `method`

##### Summary

Set an Extern variable to a float value

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the variable |
| value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The value to set |

<a name='M-PeartreeGames-Topiary-Dialogue-Set-System-String,PeartreeGames-Topiary-Function,System-Byte-'></a>
### Set(name,function,arity) `method`

##### Summary

Set a Global Extern variable to a function value
Note: It is easier to use the TopiAttribute instead with the BindFunctions method
However this is kept in case you need more control

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the variable |
| function | [PeartreeGames.Topiary.Function](#T-PeartreeGames-Topiary-Function 'PeartreeGames.Topiary.Function') | The value to set |
| arity | [System.Byte](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte 'System.Byte') | The number of parameters the function accepts |

<a name='M-PeartreeGames-Topiary-Dialogue-Start-System-String-'></a>
### Start(bough) `method`

##### Summary

Start the Dialogue

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| bough | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Optional: The bough path where the conversation will start.
If non provided, first bough in the file will be used |

<a name='M-PeartreeGames-Topiary-Dialogue-Subscribe-System-String,PeartreeGames-Topiary-Delegates-Subscriber-'></a>
### Subscribe(name,callback) `method`

##### Summary

Subscribe to when a Global variable changes

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the variable |
| callback | [PeartreeGames.Topiary.Delegates.Subscriber](#T-PeartreeGames-Topiary-Delegates-Subscriber 'PeartreeGames.Topiary.Delegates.Subscriber') | The callback to be executed on change |

<a name='M-PeartreeGames-Topiary-Dialogue-Unset-System-String-'></a>
### Unset(name) `method`

##### Summary

Set an Extern variable to a nil value

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the variable |

<a name='M-PeartreeGames-Topiary-Dialogue-Unsubscribe-System-String,PeartreeGames-Topiary-Delegates-Subscriber-'></a>
### Unsubscribe(name,callback) `method`

##### Summary

Unsubscribe when a Global variable changes

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the variable |
| callback | [PeartreeGames.Topiary.Delegates.Subscriber](#T-PeartreeGames-Topiary-Delegates-Subscriber 'PeartreeGames.Topiary.Delegates.Subscriber') | The callback that was passed into Subscribe |

<a name='T-PeartreeGames-Topiary-EmbeddedLoader'></a>
## EmbeddedLoader `type`

##### Namespace

PeartreeGames.Topiary

##### Summary

Represents a loader interface for loading and interacting with libraries.

<a name='M-PeartreeGames-Topiary-EmbeddedLoader-CreateEmbeddedResource-System-String-'></a>
### CreateEmbeddedResource(dllName) `method`

##### Summary

Creates an embedded resource from the specified DLL name.

##### Returns

The temporary file path where the embedded resource is created.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| dllName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the DLL. |

<a name='T-PeartreeGames-Topiary-Function'></a>
## Function `type`

##### Namespace

PeartreeGames.Topiary

##### Summary

Represents a function that can be called dynamically.

<a name='M-PeartreeGames-Topiary-Function-Call-System-IntPtr,System-Byte-'></a>
### Call(argPtr,count) `method`

##### Summary

Executes the delegate stored in the Function object.

##### Returns

The result of executing the delegate.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| argPtr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | A pointer to the arguments for the delegate. |
| count | [System.Byte](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte 'System.Byte') | The number of arguments. |

<a name='M-PeartreeGames-Topiary-Function-Create-System-Reflection-MethodInfo-'></a>
### Create(method) `method`

##### Summary

Creates a [Function](#T-PeartreeGames-Topiary-Function 'PeartreeGames.Topiary.Function') object based on the given [MethodInfo](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Reflection.MethodInfo 'System.Reflection.MethodInfo').

##### Returns

A new instance of [Function](#T-PeartreeGames-Topiary-Function 'PeartreeGames.Topiary.Function') created from the method.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| method | [System.Reflection.MethodInfo](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Reflection.MethodInfo 'System.Reflection.MethodInfo') | The [MethodInfo](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Reflection.MethodInfo 'System.Reflection.MethodInfo') representing the method. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.NotSupportedException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.NotSupportedException 'System.NotSupportedException') | Thrown when the number of parameters is not supported. |

<a name='M-PeartreeGames-Topiary-Function-CreateArgs-System-IntPtr,System-Byte-'></a>
### CreateArgs(argPtr,count) `method`

##### Summary

Creates an array of TopiValue objects from the given IntPtr and count.

##### Returns

An array of TopiValue objects.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| argPtr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The IntPtr pointing to the start of the memory block containing the TopiValues. |
| count | [System.Byte](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte 'System.Byte') | The number of TopiValues to create. |

<a name='M-PeartreeGames-Topiary-Function-Dispose'></a>
### Dispose() `method`

##### Summary

Disposes of the resources used by the Function instance.

##### Parameters

This method has no parameters.

<a name='M-PeartreeGames-Topiary-Function-GetCallIntPtr'></a>
### GetCallIntPtr() `method`

##### Summary

Returns the function pointer for the GetCallIntPtr method.

##### Returns

The function pointer for the GetCallIntPtr method.

##### Parameters

This method has no parameters.

<a name='M-PeartreeGames-Topiary-Function-ToString'></a>
### ToString() `method`

##### Summary

Converts the Function object to its string representation.

##### Returns

A string that represents the current Function object.

##### Parameters

This method has no parameters.

<a name='T-PeartreeGames-Topiary-ILoader'></a>
## ILoader `type`

##### Namespace

PeartreeGames.Topiary

##### Summary

Represents a loader interface for loading and interacting with libraries.

<a name='M-PeartreeGames-Topiary-ILoader-Free-System-IntPtr-'></a>
### Free(ptr) `method`

##### Summary

Frees the specified library handle.

##### Returns

`true` if the library handle is successfully freed; otherwise, `false`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ptr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The pointer to the library handle. |

<a name='M-PeartreeGames-Topiary-ILoader-GetProc-System-String-'></a>
### GetProc(name) `method`

##### Summary

Retrieves the address of the specified function from the loaded library.

##### Returns

The address of the specified function if the function is found, or IntPtr.Zero if the function is not found.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the function to retrieve. |

<a name='M-PeartreeGames-Topiary-ILoader-Load'></a>
### Load() `method`

##### Summary

Loads the library.

##### Returns

A [SafeHandle](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Runtime.InteropServices.SafeHandle 'System.Runtime.InteropServices.SafeHandle') representing the loaded library.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ComponentModel.Win32Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ComponentModel.Win32Exception 'System.ComponentModel.Win32Exception') | Thrown if the library failed to load. |

<a name='T-PeartreeGames-Topiary-Delegates-IsWaitingDelegate'></a>
## IsWaitingDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

<a name='T-PeartreeGames-Topiary-Library'></a>
## Library `type`

##### Namespace

PeartreeGames.Topiary

##### Summary

Represents a library of functions and utilities for working with dialogue systems.

<a name='M-PeartreeGames-Topiary-Library-#ctor'></a>
### #ctor() `constructor`

##### Summary

Represents a library that provides functionality for working with the Topiary dialogue system.

##### Parameters

This constructor has no parameters.

<a name='F-PeartreeGames-Topiary-Library-Loader'></a>
### Loader `constants`

##### Summary

Represents a loader for the PeartreeGames.Topiary library.

<a name='F-PeartreeGames-Topiary-Library-OnDebugLogMessage'></a>
### OnDebugLogMessage `constants`

<a name='P-PeartreeGames-Topiary-Library-Allocator'></a>
### Allocator `property`

##### Summary

Represents an allocator interface for memory management.

<a name='P-PeartreeGames-Topiary-Library-Count'></a>
### Count `property`

##### Summary

Gets the count.

<a name='P-PeartreeGames-Topiary-Library-Global'></a>
### Global `property`

##### Summary

Represents a global instance of the [Library](#T-PeartreeGames-Topiary-Library 'PeartreeGames.Topiary.Library') class.

<a name='P-PeartreeGames-Topiary-Library-IsUnityRuntime'></a>
### IsUnityRuntime `property`

##### Summary

Represents a property that indicates whether the program is running in Unity runtime or not.

<a name='M-PeartreeGames-Topiary-Library-Dispose'></a>
### Dispose() `method`

##### Summary

Releases all resources used by the object.

##### Parameters

This method has no parameters.

<a name='M-PeartreeGames-Topiary-Library-PtrToUtf8String-System-IntPtr,System-Nullable{System-Int32}-'></a>
### PtrToUtf8String(pointer,count) `method`

##### Summary

Converts a pointer to a null-terminated UTF8-encoded string into a C# string.

##### Returns

The converted C# string.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| pointer | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The pointer to the null-terminated UTF8-encoded string. |
| count | [System.Nullable{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Int32}') | Optional: The number of bytes to read from the pointer. Use null to read until null termination. |

##### Remarks

Since we're targeting .net471 for unity we need to create our own ptr to utf8 it seems

<a name='T-PeartreeGames-Topiary-Line'></a>
## Line `type`

##### Namespace

PeartreeGames.Topiary

##### Summary

Dialogue Line

<a name='P-PeartreeGames-Topiary-Line-Content'></a>
### Content `property`

##### Summary

The words spoken

<a name='P-PeartreeGames-Topiary-Line-Speaker'></a>
### Speaker `property`

##### Summary

The Speaker of the dialogue line

<a name='P-PeartreeGames-Topiary-Line-Tags'></a>
### Tags `property`

##### Summary

Array of tags

<a name='T-PeartreeGames-Topiary-Delegates-LoadStateDelegate'></a>
## LoadStateDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Delegate used to load the state of the library.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.LoadStateDelegate](#T-T-PeartreeGames-Topiary-Delegates-LoadStateDelegate 'T:PeartreeGames.Topiary.Delegates.LoadStateDelegate') | Pointer to the virtual machine. |

<a name='T-PeartreeGames-Topiary-MacLoader'></a>
## MacLoader `type`

##### Namespace

PeartreeGames.Topiary

##### Summary

Represents a loader interface for loading and interacting with libraries.

<a name='M-PeartreeGames-Topiary-MacLoader-#ctor-System-Boolean-'></a>
### #ctor() `constructor`

##### Summary

Represents a loader interface for loading and interacting with libraries.

##### Parameters

This constructor has no parameters.

<a name='M-PeartreeGames-Topiary-MacLoader-Free-System-IntPtr-'></a>
### Free(ptr) `method`

##### Summary

Frees the specified library handle.

##### Returns

`true` if the library handle is successfully freed; otherwise, `false`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ptr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The pointer to the library handle. |

##### Example

```
IntPtr libraryHandle = LoadLibrary("example.dll");
bool result = Free(libraryHandle);
```

<a name='M-PeartreeGames-Topiary-MacLoader-GetProc-System-String-'></a>
### GetProc(name) `method`

##### Summary

Retrieves the address of the specified function from the loaded library.

##### Returns

The address of the specified function if the function is found, or IntPtr.Zero if the function is not found.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the function to retrieve. |

<a name='M-PeartreeGames-Topiary-MacLoader-Load'></a>
### Load() `method`

##### Summary

Loads the library by calling the underlying native method dlopen.

##### Returns

A SafeHandle object representing the loaded library.

##### Parameters

This method has no parameters.

<a name='M-PeartreeGames-Topiary-MacLoader-ReleaseHandle'></a>
### ReleaseHandle() `method`

##### Summary

Releases the handle of the library.

##### Returns

`true` if the handle is successfully released; otherwise, `false`.

##### Parameters

This method has no parameters.

<a name='T-PeartreeGames-Topiary-Dialogue-OnChoicesCallback'></a>
## OnChoicesCallback `type`

##### Namespace

PeartreeGames.Topiary.Dialogue

##### Summary

Represents a callback function for handling choices in a dialogue.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| dialogue | [T:PeartreeGames.Topiary.Dialogue.OnChoicesCallback](#T-T-PeartreeGames-Topiary-Dialogue-OnChoicesCallback 'T:PeartreeGames.Topiary.Dialogue.OnChoicesCallback') | The dialogue in which the choice is being made. |

<a name='T-PeartreeGames-Topiary-Delegates-OnChoicesDelegate'></a>
## OnChoicesDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Represents a delegate that handles the event when choices are presented to the user.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.OnChoicesDelegate](#T-T-PeartreeGames-Topiary-Delegates-OnChoicesDelegate 'T:PeartreeGames.Topiary.Delegates.OnChoicesDelegate') | A pointer to the virtual machine instance. |

<a name='T-PeartreeGames-Topiary-Dialogue-OnLineCallback'></a>
## OnLineCallback `type`

##### Namespace

PeartreeGames.Topiary.Dialogue

##### Summary

Represents a callback method that processes each line of dialogue.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| dialogue | [T:PeartreeGames.Topiary.Dialogue.OnLineCallback](#T-T-PeartreeGames-Topiary-Dialogue-OnLineCallback 'T:PeartreeGames.Topiary.Dialogue.OnLineCallback') | The Dialogue object that invoked the callback. |

<a name='T-PeartreeGames-Topiary-Delegates-OnLineDelegate'></a>
## OnLineDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Represents a delegate used for handling an event when a line is encountered during dialogue execution.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.OnLineDelegate](#T-T-PeartreeGames-Topiary-Delegates-OnLineDelegate 'T:PeartreeGames.Topiary.Delegates.OnLineDelegate') | The pointer to the virtual machine. |

<a name='T-PeartreeGames-Topiary-Delegates-RunDelegate'></a>
## RunDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Represents a delegate that is used to run the library.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.RunDelegate](#T-T-PeartreeGames-Topiary-Delegates-RunDelegate 'T:PeartreeGames.Topiary.Delegates.RunDelegate') | The pointer to the VM. |

<a name='T-PeartreeGames-Topiary-Delegates-SaveStateDelegate'></a>
## SaveStateDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Represents a delegate for saving the state of a virtual machine.

##### Returns

The number of bytes written to the output buffer.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.SaveStateDelegate](#T-T-PeartreeGames-Topiary-Delegates-SaveStateDelegate 'T:PeartreeGames.Topiary.Delegates.SaveStateDelegate') | A pointer to the virtual machine. |

<a name='T-PeartreeGames-Topiary-Delegates-SelectChoiceDelegate'></a>
## SelectChoiceDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Delegate for selecting a choice in the game.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.SelectChoiceDelegate](#T-T-PeartreeGames-Topiary-Delegates-SelectChoiceDelegate 'T:PeartreeGames.Topiary.Delegates.SelectChoiceDelegate') | A pointer to the virtual machine. |

<a name='T-PeartreeGames-Topiary-Delegates-SelectContinueDelegate'></a>
## SelectContinueDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Represents a delegate used to select the continue option.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.SelectContinueDelegate](#T-T-PeartreeGames-Topiary-Delegates-SelectContinueDelegate 'T:PeartreeGames.Topiary.Delegates.SelectContinueDelegate') | The pointer to the virtual machine. |

<a name='T-PeartreeGames-Topiary-Delegates-SetDebugLogDelegate'></a>
## SetDebugLogDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Represents a delegate for setting the debug log.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| logPtr | [T:PeartreeGames.Topiary.Delegates.SetDebugLogDelegate](#T-T-PeartreeGames-Topiary-Delegates-SetDebugLogDelegate 'T:PeartreeGames.Topiary.Delegates.SetDebugLogDelegate') | The pointer to the debug log function. |

<a name='T-PeartreeGames-Topiary-Delegates-SetDebugSeverityDelegate'></a>
## SetDebugSeverityDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

<a name='T-PeartreeGames-Topiary-Delegates-SetExternBoolDelegate'></a>
## SetExternBoolDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Delegate for setting an extern boolean value.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.SetExternBoolDelegate](#T-T-PeartreeGames-Topiary-Delegates-SetExternBoolDelegate 'T:PeartreeGames.Topiary.Delegates.SetExternBoolDelegate') | The pointer to the virtual machine. |

<a name='T-PeartreeGames-Topiary-Delegates-SetExternFuncDelegate'></a>
## SetExternFuncDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Represents a delegate that can be used to set an external function in the Library class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.SetExternFuncDelegate](#T-T-PeartreeGames-Topiary-Delegates-SetExternFuncDelegate 'T:PeartreeGames.Topiary.Delegates.SetExternFuncDelegate') | The pointer to the virtual machine. |

<a name='T-PeartreeGames-Topiary-Delegates-SetExternNilDelegate'></a>
## SetExternNilDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Represents a delegate used to set an extern value to nil.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.SetExternNilDelegate](#T-T-PeartreeGames-Topiary-Delegates-SetExternNilDelegate 'T:PeartreeGames.Topiary.Delegates.SetExternNilDelegate') | A pointer to the virtual machine. |

<a name='T-PeartreeGames-Topiary-Delegates-SetExternNumberDelegate'></a>
## SetExternNumberDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Delegate for the function that sets an external number in the library.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.SetExternNumberDelegate](#T-T-PeartreeGames-Topiary-Delegates-SetExternNumberDelegate 'T:PeartreeGames.Topiary.Delegates.SetExternNumberDelegate') | Pointer to the virtual machine instance. |

<a name='T-PeartreeGames-Topiary-Delegates-SetExternStringDelegate'></a>
## SetExternStringDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Delegate for setting an external string in the VM.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.SetExternStringDelegate](#T-T-PeartreeGames-Topiary-Delegates-SetExternStringDelegate 'T:PeartreeGames.Topiary.Delegates.SetExternStringDelegate') | Pointer to the VM. |

<a name='T-PeartreeGames-Topiary-Library-Severity'></a>
## Severity `type`

##### Namespace

PeartreeGames.Topiary.Library

##### Summary

Represents the severity level of a log message.

<a name='T-PeartreeGames-Topiary-Delegates-StartDelegate'></a>
## StartDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Delegate for the Start method in the PeartreeGames.Topiary library.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.StartDelegate](#T-T-PeartreeGames-Topiary-Delegates-StartDelegate 'T:PeartreeGames.Topiary.Delegates.StartDelegate') | A pointer to the virtual machine instance. |

<a name='T-PeartreeGames-Topiary-Delegates-SubscribeDelegate'></a>
## SubscribeDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Represents a delegate used to subscribe to events in the Topiary library.

##### Returns

True if the subscription is successful, otherwise false.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.SubscribeDelegate](#T-T-PeartreeGames-Topiary-Delegates-SubscribeDelegate 'T:PeartreeGames.Topiary.Delegates.SubscribeDelegate') | The pointer to the VM. |

<a name='T-PeartreeGames-Topiary-Delegates-Subscriber'></a>
## Subscriber `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Provides methods to subscribe and unsubscribe to dialogue events.

<a name='T-PeartreeGames-Topiary-TopiValue-Tag'></a>
## Tag `type`

##### Namespace

PeartreeGames.Topiary.TopiValue

##### Summary

Represents the different types of tags for the TopiValue struct.

<a name='F-PeartreeGames-Topiary-TopiValue-Tag-Bool'></a>
### Bool `constants`

##### Summary

Represents a boolean value in the Topiary framework.

<a name='F-PeartreeGames-Topiary-TopiValue-Tag-List'></a>
### List `constants`

##### Summary

Represents the different tags for the TopiValue enum.

<a name='F-PeartreeGames-Topiary-TopiValue-Tag-Map'></a>
### Map `constants`

##### Summary

Represents a member of the enum Tag with the value Map.

<a name='F-PeartreeGames-Topiary-TopiValue-Tag-Nil'></a>
### Nil `constants`

##### Summary

Represents the Nil tag of the [Tag](#T-PeartreeGames-Topiary-TopiValue-Tag 'PeartreeGames.Topiary.TopiValue.Tag') enum.

<a name='F-PeartreeGames-Topiary-TopiValue-Tag-Number'></a>
### Number `constants`

##### Summary

Represents a numeric value in the TopiValue enum.

<a name='F-PeartreeGames-Topiary-TopiValue-Tag-Set'></a>
### Set `constants`

##### Summary

Represents a member of the enum Tag that signifies a Set type.

<a name='F-PeartreeGames-Topiary-TopiValue-Tag-String'></a>
### String `constants`

##### Summary

Represents a string value in the [TopiValue](#T-PeartreeGames-Topiary-TopiValue 'PeartreeGames.Topiary.TopiValue') enum.

<a name='T-PeartreeGames-Topiary-TopiAttribute'></a>
## TopiAttribute `type`

##### Namespace

PeartreeGames.Topiary

##### Summary

Represents an attribute that declares a method as an extern topi function.
Can only be used on static methods.

<a name='M-PeartreeGames-Topiary-TopiAttribute-#ctor-System-String-'></a>
### #ctor(name) `constructor`

##### Summary

Declare the function as an extern topi function
Can only be used on static methods

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Name of the function in the topi file |

<a name='P-PeartreeGames-Topiary-TopiAttribute-Name'></a>
### Name `property`

##### Summary

Gets or sets the name of the function in the topi file.

<a name='T-PeartreeGames-Topiary-TopiValue'></a>
## TopiValue `type`

##### Namespace

PeartreeGames.Topiary

##### Summary

Topiary Value container
Data is overlapped in memory so ensure correct value is used
or check tag if unknown

<a name='M-PeartreeGames-Topiary-TopiValue-AsType``1'></a>
### AsType\`\`1() `method`

##### Summary

Gets the value of the TopiValue as the specified type.
Will create boxing, better to use the above is value type is known

##### Returns

The value of the TopiValue as type T.

##### Parameters

This method has no parameters.

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type to convert the value to. |

##### Remarks

This method converts the underlying value of the TopiValue to the specified type T.
If the conversion is not possible, an InvalidCastException will be thrown.

<a name='M-PeartreeGames-Topiary-TopiValue-Dispose'></a>
### Dispose() `method`

##### Summary

Releases the resources used by the TopiValue.

##### Parameters

This method has no parameters.

<a name='M-PeartreeGames-Topiary-TopiValue-Equals-PeartreeGames-Topiary-TopiValue-'></a>
### Equals(other) `method`

##### Summary

Determines whether the current instance is equal to the specified object.

##### Returns

True if the current instance is equal to the specified object; otherwise, false.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| other | [PeartreeGames.Topiary.TopiValue](#T-PeartreeGames-Topiary-TopiValue 'PeartreeGames.Topiary.TopiValue') | The object to compare with the current instance. |

<a name='M-PeartreeGames-Topiary-TopiValue-Equals-System-Object-'></a>
### Equals(obj) `method`

##### Summary

Determines whether the current instance is equal to another TopiValue object.

##### Returns

true if the current instance is equal to the other object; otherwise, false.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| obj | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | The TopiValue object to compare with the current instance. |

<a name='M-PeartreeGames-Topiary-TopiValue-FromPtr-System-IntPtr-'></a>
### FromPtr(ptr) `method`

##### Summary

Converts a pointer to a TopiValue struct.

##### Returns

The converted TopiValue.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ptr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The pointer to the TopiValue struct. |

<a name='M-PeartreeGames-Topiary-TopiValue-GetHashCode'></a>
### GetHashCode() `method`

##### Summary

Gets the hash code of the TopiValue object.

##### Returns

The hash code of the TopiValue object.

##### Parameters

This method has no parameters.

<a name='M-PeartreeGames-Topiary-TopiValue-ToString'></a>
### ToString() `method`

##### Summary

Converts the TopiValue object to its string representation.

##### Returns

The string representation of the TopiValue object.

##### Parameters

This method has no parameters.

<a name='T-PeartreeGames-Topiary-Delegates-TryGetValueDelegate'></a>
## TryGetValueDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Delegate for the TryGetValue function.

##### Returns

True if the value was successfully retrieved, false otherwise.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.TryGetValueDelegate](#T-T-PeartreeGames-Topiary-Delegates-TryGetValueDelegate 'T:PeartreeGames.Topiary.Delegates.TryGetValueDelegate') | The pointer to the virtual machine. |

<a name='T-PeartreeGames-Topiary-Delegates-UnsubscribeDelegate'></a>
## UnsubscribeDelegate `type`

##### Namespace

PeartreeGames.Topiary.Delegates

##### Summary

Represents a delegate used for unregistering a callback function from the library.

##### Returns

true if the callback function was successfully unregistered; otherwise, false.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vmPtr | [T:PeartreeGames.Topiary.Delegates.UnsubscribeDelegate](#T-T-PeartreeGames-Topiary-Delegates-UnsubscribeDelegate 'T:PeartreeGames.Topiary.Delegates.UnsubscribeDelegate') | A pointer to the virtual machine instance. |

<a name='T-PeartreeGames-Topiary-WindowsLoader'></a>
## WindowsLoader `type`

##### Namespace

PeartreeGames.Topiary

##### Summary

Represents a loader interface for loading and interacting with libraries.

<a name='M-PeartreeGames-Topiary-WindowsLoader-#ctor-System-Boolean-'></a>
### #ctor() `constructor`

##### Summary

Represents a loader interface for loading and interacting with libraries.

##### Parameters

This constructor has no parameters.

<a name='M-PeartreeGames-Topiary-WindowsLoader-Free-System-IntPtr-'></a>
### Free(ptr) `method`

##### Summary

Frees the specified library handle.

##### Returns

`true` if the library handle is successfully freed; otherwise, `false`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ptr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The pointer to the library handle. |

##### Example

```
IntPtr libraryHandle = LoadLibrary("example.dll");
bool result = Free(libraryHandle);
```

<a name='M-PeartreeGames-Topiary-WindowsLoader-GetProc-System-String-'></a>
### GetProc(name) `method`

##### Summary

Retrieves the address of the specified function from the loaded library.

##### Returns

The address of the specified function if the function is found, or IntPtr.Zero if the function is not found.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the function to retrieve. |

<a name='M-PeartreeGames-Topiary-WindowsLoader-Load'></a>
### Load() `method`

##### Summary

Loads the library.

##### Returns

The safe handle of the loaded library.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ComponentModel.Win32Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ComponentModel.Win32Exception 'System.ComponentModel.Win32Exception') | Thrown when the library failed to load. |

<a name='M-PeartreeGames-Topiary-WindowsLoader-ReleaseHandle'></a>
### ReleaseHandle() `method`

##### Summary

Releases the handle of the library.

##### Returns

`true` if the handle is successfully released; otherwise, `false`.

##### Parameters

This method has no parameters.
