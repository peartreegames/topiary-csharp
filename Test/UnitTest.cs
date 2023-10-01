using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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
            var offset = 0;
            for (var i = 0; i < length; i++)
            {
                var ptr = Marshal.ReadIntPtr(choicesPtr, offset);
                choices[i] = Marshal.PtrToStructure<Choice>(ptr);
                offset += IntPtr.Size;
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

        [Test]
        public void CreateVm()
        {
            var data = File.ReadAllBytes("./test.topib");
            var vmPtr = Library.InitVm(data, OnDialogue, OnChoices);
            var print = new Library.Subscriber(Print);
            Library.Subscribe(vmPtr, "value", print);
            Library.Run(vmPtr);
            Library.Unsubscribe(vmPtr, "value", print);
            var list = Library.GetValue(vmPtr, "list");
            Console.WriteLine($"LIST VALUE:: {list.tag} = {list.Value}");
            Library.DestroyValue(ref list);
            Library.DestroyVm(vmPtr);
        }
    }
}