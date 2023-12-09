using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Topiary
{
    public class Story : IDisposable
    {
        private IntPtr _vmPtr;
        private readonly StringBuilder _errMsg;
        private readonly OnDialogueCallback _onDialogue;
        private readonly OnChoicesCallback _onChoices;
        public delegate void OnDialogueCallback(Story story, Dialogue dialogue);

        public delegate void OnChoicesCallback(Story story, Choice[] choices);

        public Story(byte[] source, OnDialogueCallback onDialogue, OnChoicesCallback onChoices)
        {
            _onDialogue = onDialogue;
            _onChoices = onChoices;
            _vmPtr = Library.InitVm(source, OnDialogue, OnChoices);
            _errMsg = new StringBuilder(2048);
        }

        public void Dispose()
        {
            if (_vmPtr == IntPtr.Zero) return;
            Library.destroyVm(_vmPtr);
            _vmPtr = IntPtr.Zero;
        }

        private void OnDialogue(IntPtr vmPtr, Dialogue dialogue)
        {
            _onDialogue(this, dialogue);
        }

        private void OnChoices(IntPtr vmPtr, IntPtr choicesPtr, byte count)
        {
            _onChoices(this, Choice.MarshalPtr(choicesPtr, count));
        }

        /// <summary>
        /// Start the Story
        /// </summary>
        public void Start()
        {
            Library.start(_vmPtr);
        }
        /// <summary>
        /// Run the Story until the next Dialogue or Choice
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Run()
        {
            Library.run(_vmPtr, out var errLine, _errMsg, _errMsg.Capacity);
            if (errLine != 0)
            {
                throw new Exception($"Topiary Error line {errLine}: {_errMsg}");
            }
        }

        /// <summary>
        /// Continue the Story.
        /// </summary>
        public void Continue() => Library.selectContinue(_vmPtr);

        public bool CanContinue() => Library.canContinue(_vmPtr);

        public bool IsWaiting() => Library.isWaiting(_vmPtr);

        /// <summary>
        /// Select a Choice.
        /// </summary>
        /// <param name="index">The index of the choice selected</param>
        public void SelectChoice(int index) => Library.selectChoice(_vmPtr, index);


        /// <summary>
        /// Retrieve the current value of any Global variable in the story
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <returns>TopiValue.nil if not found</returns>
        public TopiValue GetValue(string name)
        {
            if (!Library.tryGetValue(_vmPtr, name, name.Length, out var value))
                Console.WriteLine($"Cannot find value: {name}");
            return value;
        }

        /// <summary>
        /// Destroy a reference value in unmanaged memory
        /// NOTE: This should be removed in future versions
        /// </summary>
        /// <param name="value">The value to be destroyed</param>
        public void DestroyValue(ref TopiValue value) => Library.destroyValue(ref value);

        /// <summary>
        /// Subscribe to when a Global variable changes
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="callback">The callback to be executed on change</param>
        public void Subscribe(string name, Library.Subscriber callback) => Library.subscribe(_vmPtr, name,
            name.Length, Marshal.GetFunctionPointerForDelegate(callback));

        /// <summary>
        /// Unsubscribe when a Global variable changes
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="callback">The callback that was passed into Subscribe</param>
        public void Unsubscribe(string name, Library.Subscriber callback) => Library.unsubscribe(_vmPtr,
            name,
            name.Length, Marshal.GetFunctionPointerForDelegate(callback));

        /// <summary>
        /// Set an Extern variable to a bool value
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, bool value) => Library.setExternBool(_vmPtr, name, name.Length, value);

        /// <summary>
        /// Set an Extern variable to a float value
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, float value) => Library.setExternNumber(_vmPtr, name, name.Length, value);

        /// <summary>
        /// Set an Extern variable to a float value
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value to set</param>
        public void Set(string name, string value) =>
            Library.setExternString(_vmPtr, name, name.Length, value, value.Length);

        /// <summary>
        /// Set a Global Extern variable to a function value
        /// Note: It is easier to use the TopiAttribute instead with the BindFunctions method
        /// However this is kept in case you need more control 
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="function">The value to set</param>
        /// <param name="arity">The number of parameters the function accepts</param>
        public void Set(string name, Library.ExternFunctionDelegate function, byte arity) =>
            Library.setExternFunc(_vmPtr, name, name.Length, Marshal.GetFunctionPointerForDelegate(function), arity);

        /// <summary>
        /// Set an Extern variable to a nil value
        /// </summary>
        /// <param name="name">The name of the variable</param>
        public void Unset(string name) => Library.setExternNil(_vmPtr, name, name.Length);

        /// <summary>
        /// Bind all TopiAttribute functions within the given Assemblies
        /// Functions must be of type "Func&lt;TopiValue[,TopiValue,TopiValue,TopiValue,TopiValue]&gt;"
        /// or "Action[&lt;TopiValue,TopiValue,TopiValue,TopiValue&gt;]"
        /// See <see cref="Function"/>
        /// </summary>
        /// <param name="assemblies"></param>
        public void BindFunctions(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                if (Regex.IsMatch(assembly.FullName, "^(System|Microsoft|mscorlib|netstandard|Windows|JetBrains|test)"))
                    continue;
                foreach (var type in assembly.DefinedTypes)
                {
                    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static |
                                                           BindingFlags.NonPublic))
                    {
                        if (!(method.GetCustomAttribute(typeof(TopiAttribute), false) is TopiAttribute attr)) continue;
                        var del = Function.CreateFromMethodInfo(method);
                        Set(attr.Name, del, (byte) method.GetParameters().Length);
                    }
                }
            }
        }
    }
}