// #define ASYNC

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Topiary;

namespace Test
{
    public class Tests
    {
        private static void OnDialogue(IntPtr vmPtr, Dialogue dialogue)
        {
            Console.WriteLine($":{dialogue.Speaker}: {dialogue.Content} {string.Join('#', dialogue.Tags)}");
#if ASYNC
            ContinueAsync(vmPtr);
#else
            Library.Continue(vmPtr);
#endif
        }

        private static async void ContinueAsync(IntPtr vmPtr)
        {
            await Task.Delay(200);
            Library.Continue(vmPtr);
        }

        private static void OnChoices(IntPtr vmPtr, Choice[] choices)
        {
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

        [Topi("sqrPrint")]
        private static void SqrPrint(TopiValue value)
        {
            var i = value.Int;
            Console.WriteLine($"SqrPrint:: {i * i}");
        }

        [Topi("sqr")]
        public static TopiValue Sqr(TopiValue value)
        {
            var i = value.Int;
            return new TopiValue(i * i);
        }

        [Test]
        public void CreateVm()
        {
            var data = File.ReadAllBytes("./test.topib");
            var vmPtr = Library.InitVm(data, OnDialogue, OnChoices);
            Library.BindFunctions(vmPtr, new []{typeof(Tests).Assembly});
            var print = new Library.Subscriber(Print);
            Library.Subscribe(vmPtr, "value", print);
            var err = new StringBuilder(1028);
            Library.Run(vmPtr, out var errLine, err, err.Capacity);
            if (errLine != 0)
            {
                Console.WriteLine($"Error line {errLine}: {err}");
                Library.DestroyVm(vmPtr);
                return;
            }

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