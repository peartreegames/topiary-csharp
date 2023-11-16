using System;
using System.IO;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Topiary;

namespace Test
{
    public class Tests
    {
        private static void OnDialogue(IntPtr vmPtr, Dialogue dialogue)
        {
            Console.WriteLine($":{dialogue.Speaker}: {dialogue.Content} {string.Join('#', dialogue.Tags)}");
            Library.Continue(vmPtr);
        }

        private static void OnChoices(IntPtr vmPtr, IntPtr choicesPtr, byte length)
        {
            var choices = new Choice[length];
            var ptr = choicesPtr;
            for (var i = 0; i < length; i++)
            {
                choices[i] = Marshal.PtrToStructure<Choice>(ptr);
                ptr = IntPtr.Add(ptr, Marshal.SizeOf<Choice>());
            }

            foreach (var choice in choices)
            {
                Console.WriteLine($"{choice.Content}");
            }

            var index = new Random(DateTime.Now.Millisecond).Next(0, choices.Length);
            Console.WriteLine($"Choice: {index}");
            Library.SelectChoice(vmPtr, index);
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Compile()
        {
            var text = File.ReadAllText("./test.topi");
            var compiled = Library.Compile(text);
            File.WriteAllBytes("./test.topib", compiled);
            Assert.That(Path.Exists("./test.topib"), Is.True);
        }

        private static void Print(ref TopiValue value) =>
            Console.WriteLine($"PRINT:: {value.tag} = {value.Value}");

        private static TopiValue SqrPrint(IntPtr args, byte len)
        {
            var arg = Marshal.PtrToStructure<TopiValue>(args);
            Console.WriteLine(arg.data.numberValue * arg.data.numberValue);
            return default;
        }

        [Test]
        public void CreateVm()
        {
            var data = File.ReadAllBytes("./test.topib");
            var vmPtr = Library.InitVm(data, OnDialogue, OnChoices);
            Library.SetExternFunction(vmPtr, "sqrPrint", SqrPrint);
            var print = new Library.Subscriber(Print);
            Library.Subscribe(vmPtr, "value", print);
            Library.Run(vmPtr);
            Library.Unsubscribe(vmPtr, "value", print);
            var list = Library.GetValue(vmPtr, "list");
            Console.WriteLine($"{list.tag} = {list}");
            Library.DestroyValue(ref list);
            var set = Library.GetValue(vmPtr, "set");
            Console.WriteLine($"{set.tag} = {set}");
            Library.DestroyValue(ref set);
            var map = Library.GetValue(vmPtr, "map");
            Console.WriteLine($"{map.tag} = {map}");
            Library.DestroyValue(ref map);
            Library.DestroyVm(vmPtr);
        }
    }
}