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
            Library.Compile("./test.topi");
            Assert.That(Path.Exists("./test.topib"), Is.True);
        }

        private static void Print(ref TopiValue value) => 
            Console.WriteLine($"PRINT:: {value.tag} = {value.Value}");
        
        [Test]
        public void CreateVm()
        {
            var vmPtr = Library.InitVm("./test.topib", OnDialogue, OnChoices);
            var print = new Library.Subscriber(Print);
            Library.Subscribe(vmPtr, "value", print);
            Library.Run(vmPtr);
            Library.Unsubscribe(vmPtr, "value", print);
            var value = Library.GetVariable(vmPtr, "value");
            Console.WriteLine($"VALUE FINAL:: {value.tag} = {value.Value}");
            Library.DestroyVm(vmPtr);
        }
    }
}