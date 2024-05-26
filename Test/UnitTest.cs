using System;
using System.IO;
using NUnit.Framework;

namespace PeartreeGames.Topiary.Test
{
    public class Tests
    {
        private static string? _state;

        private static void OnLine(IntPtr vmPtr, Line line)
        {
            Console.Write($":{line.Speaker}: {line.Content} ");
            foreach (var tag in line.Tags) Console.Write($"#{tag} ");
            Console.Write("\n");
            Dialogue.Dialogues[vmPtr].Continue();
        }

        private static void OnChoices(IntPtr vmPtr, IntPtr choicesPtr, byte count)
        {
            var choices = Choice.MarshalPtr(choicesPtr, count);
            foreach (var choice in choices)
            {
                Console.Write($">>> {choice.Content} ");
                foreach (var tag in choice.Tags) Console.Write($"#{tag} ");
                Console.Write("\n");
            }

            var index = new Random(DateTime.Now.Millisecond).Next(0, choices.Length);
            Console.WriteLine($"Random Choice: {index}");
            Dialogue.Dialogues[vmPtr].SelectChoice(index);
        }


        [SetUp]
        public void Setup()
        {
        }

        public void CompileAndRun()
        {
            Compile();
            Run();
            RunLoaded();
        }

        [Test]
        public void Compile()
        {
            var compiled = Dialogue.Compile(Path.GetFullPath("./test.topi"), Library.Log);
            Assert.That(compiled, Is.Not.Empty);
            File.WriteAllBytes("./test.topib", compiled);
            Assert.That(Path.Exists("./test.topib"), Is.True);
        }

        private static void Print(ref TopiValue value) =>
            Console.WriteLine($"PRINT:: {value.tag} = {value.Value}");

        [Topi("strPrint", 1)]
        private static TopiValue StrPrint(IntPtr argsPtr, byte count)
        {
            var value = TopiValue.CreateArgs(argsPtr, count)[0];
            var str = value.String;
            Console.WriteLine($"StrPrint:: {str}");
            return default;
        }

        [Topi("sqrPrint", 2)]
        private static TopiValue SqrPrint(IntPtr argsPtr, byte count)
        {
            var value = TopiValue.CreateArgs(argsPtr, count)[0];
            var i = value.Int;
            Console.WriteLine($"SqrPrint:: {i * i}");
            return default;
        }


        [Topi("sumPrint", 2)]
        private static TopiValue SumPrint(IntPtr argsPtr, byte count)
        {
            var args = TopiValue.CreateArgs(argsPtr, count);
            var a = args[0];
            var b = args[1];
            Console.WriteLine($"SumPrint:: {a.Float + b.Float}");
            return default;
        }


        [Topi("sqr", 1)]
        private static TopiValue Sqr(IntPtr argsPtr, byte count)
        {
            var value = TopiValue.CreateArgs(argsPtr, count)[0];
            var i = value.Int;
            return new TopiValue(i * i);
        }

        [Test]
        public void Run()
        {
            var data = File.ReadAllBytes("./test.topib");
            var dialogue = new Dialogue(data, OnLine, OnChoices, Library.Log, Library.Severity.Debug);
            dialogue.Set(Sqr);
            dialogue.Set(SqrPrint);
            dialogue.Set(StrPrint);
            dialogue.Set(SumPrint);
            
            var print = new Delegates.Subscriber(Print);
            dialogue.Subscribe("value", print);
            dialogue.Subscribe("nope", print);
            try
            {
                dialogue.Start();
                while (dialogue.CanContinue)
                {
                    dialogue.Run();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            dialogue.Unsubscribe("value", print);
            using var list = dialogue.GetValue("list");
            Console.WriteLine($"{list.tag} = {list}");
            using var set = dialogue.GetValue("set");
            Console.WriteLine($"{set.tag} = {set}");
            using var map = dialogue.GetValue("map");
            Console.WriteLine($"{map.tag} = {map}");
            _state = dialogue.SaveState();
        }

        [Test]
        public void RunLoaded()
        {
            Console.WriteLine(_state);
            var data = File.ReadAllBytes("./test.topib");
            var dialogue = new Dialogue(data, OnLine, OnChoices, Library.Log, Library.Severity.Debug);
            dialogue.LoadState(_state);
            using var list = dialogue.GetValue("list");
            Console.WriteLine($"{list.tag} = {list}");
            using var set = dialogue.GetValue("set");
            Console.WriteLine($"{set.tag} = {set}");
            using var map = dialogue.GetValue("map");
            Console.WriteLine($"{map.tag} = {map}");
        }

    }
}