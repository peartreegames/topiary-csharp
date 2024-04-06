using System;
using System.IO;
using NUnit.Framework;

namespace PeartreeGames.Topiary.Test
{
    public class Tests
    {
        private static string? _state;

        private static void OnLine(Dialogue dialogue, Line line)
        {
            Console.Write($":{line.Speaker}: {line.Content} ");
            foreach (var tag in line.Tags) Console.Write($"#{tag} ");
            Console.Write("\n");
            dialogue.Continue();
        }

        private static void OnChoices(Dialogue dialogue, Choice[] choices)
        {
            foreach (var choice in choices)
            {
                Console.Write($">>> {choice.Content} ");
                foreach (var tag in choice.Tags) Console.Write($"#{tag} ");
                Console.Write("\n");
            }

            var index = new Random(DateTime.Now.Millisecond).Next(0, choices.Length);
            Console.WriteLine($"Random Choice: {index}");
            dialogue.SelectChoice(index);
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
            Library.OnDebugLogMessage -= LogMsg;
            Library.OnDebugLogMessage += LogMsg;
            var compiled = Dialogue.Compile(Path.GetFullPath("./test.topi"));
            Assert.That(compiled, Is.Not.Empty);
            File.WriteAllBytes("./test.topib", compiled);
            Assert.That(Path.Exists("./test.topib"), Is.True);
            Library.OnDebugLogMessage -= LogMsg;
        }

        private static void Print(ref TopiValue value) =>
            Console.WriteLine($"PRINT:: {value.tag} = {value.Value}");

        [Topi("strPrint")]
        private static void StrPrint(TopiValue value)
        {
            var str = value.String;
            Console.WriteLine($"StrPrint:: {str}");
        }

        [Topi("sqrPrint")]
        private static void SqrPrint(TopiValue value)
        {
            var i = value.Int;
            Console.WriteLine($"SqrPrint:: {i * i}");
        }


        [Topi("sumPrint")]
        private static void SumPrint(TopiValue a, TopiValue b)
        {
            Console.WriteLine($"SumPrint:: {a.Float + b.Float}");
        }


        [Topi("sqr")]
        private static TopiValue Sqr(TopiValue value)
        {
            var i = value.Int;
            return new TopiValue(i * i);
        }

        [Test]
        public void Run()
        {
            var data = File.ReadAllBytes("./test.topib");
            var dialogue = new Dialogue(data, OnLine, OnChoices, Library.Severity.Warn);
            Library.OnDebugLogMessage += LogMsg;
            dialogue.BindFunctions(new[] {typeof(Tests).Assembly});
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
            var dialogue = new Dialogue(data, OnLine, OnChoices, Library.Severity.Warn);
            Library.OnDebugLogMessage += LogMsg;
            dialogue.BindFunctions(new[] {typeof(Tests).Assembly});
            dialogue.LoadState(_state);
            using var list = dialogue.GetValue("list");
            Console.WriteLine($"{list.tag} = {list}");
            using var set = dialogue.GetValue("set");
            Console.WriteLine($"{set.tag} = {set}");
            using var map = dialogue.GetValue("map");
            Console.WriteLine($"{map.tag} = {map}");
        }

        private static void LogMsg(string msg, Library.Severity severity)
        {
            switch (severity)
            {
                case Library.Severity.Debug:
                case Library.Severity.Info:
                    Console.WriteLine(msg);
                    break;
                case Library.Severity.Warn:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(msg);
                    break;
                case Library.Severity.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(msg);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
            }

            Console.ResetColor();
        }
    }
}