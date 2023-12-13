// #define ASYNC

using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Topiary;

namespace Test
{
    public class Tests
    {
        private static void OnDialogue(Story story, Dialogue dialogue)
        {
            Console.WriteLine($":{dialogue.Speaker}: {dialogue.Content} {string.Join('#', dialogue.Tags)}");
            story.Continue();
        }

        private static async void ContinueAsync(IntPtr vmPtr)
        {
            await Task.Delay(200);
        }
        
        private static void OnChoices(Story story, Choice[] choices)
        {
            foreach (var choice in choices)
            {
                Console.WriteLine($">>> {choice.Content}");
            }

            var index = new Random(DateTime.Now.Millisecond).Next(0, choices.Length);
            Console.WriteLine($"Choice: {index}");
            story.SelectChoice(index);
        }


        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Compile()
        {
            var text = File.ReadAllText("./test.topi");
            Story.DllPath = Library.Loader.DefaultDllPath; 
            var compiled = Story.Compile(text);
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
        private static TopiValue Sqr(TopiValue value)
        {
            var i = value.Int;
            return new TopiValue(i * i);
        }

        [Test]
        public void CreateVm()
        {
            var data = File.ReadAllBytes("./test.topib");
            var story = new Story(data, OnDialogue, OnChoices, Console.WriteLine);
            story.BindFunctions(new[] {typeof(Tests).Assembly});
            var print = new Library.Subscriber(Print);
            story.Subscribe( "value", print);
            try
            {
                story.Start();
                while (story.CanContinue())
                {
                    story.Run();
                }
            }
            catch (Exception e)
            { 
                Console.WriteLine(e);
            }

            story.Unsubscribe("value", print);
            var list = story.GetValue("list");
            Console.WriteLine($"{list.tag} = {list}");
            story.DestroyValue(ref list);
            var set = story.GetValue("set");
            Console.WriteLine($"{set.tag} = {set}");
            story.DestroyValue(ref set);
            var map = story.GetValue("map");
            Console.WriteLine($"{map.tag} = {map}");
            story.DestroyValue(ref map);
        }
    }
}