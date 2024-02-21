using System;
using System.IO;
using NUnit.Framework;

namespace PeartreeGames.Topiary.Test
{
    public class Tests
    {
        private static void OnDialogue(Story story, Dialogue dialogue)
        {
            Console.WriteLine(
                $":{dialogue.Speaker}: {dialogue.Content} {string.Join('#', dialogue.Tags)}");
            story.Continue();
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
        public void CompileAndRun()
        {
            Compile();
            Run();
        }


        private static void Compile()
        {
            Library.OnDebugLogMessage -= LogMsg;
            Library.OnDebugLogMessage += LogMsg;
            var compiled = Story.Compile(Path.GetFullPath("./test.topi"), Library.Severity.Debug);
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

        private static void Run()
        {
            var data = File.ReadAllBytes("./test.topib");
            var story = new Story(data, OnDialogue, OnChoices, Library.Severity.Info);
            Library.OnDebugLogMessage += LogMsg;
            story.BindFunctions(new[] {typeof(Tests).Assembly});
            var print = new Delegates.Subscriber(Print);
            story.Subscribe("value", print);
            story.Subscribe("nope", print);
            try
            {
                story.Start();
                while (story.CanContinue)
                {
                    story.Run();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            story.Unsubscribe("value", print);
            using var list = story.GetValue("list");
            Console.WriteLine($"{list.tag} = {list}");
            using var set = story.GetValue("set");
            Console.WriteLine($"{set.tag} = {set}");
            using var map = story.GetValue("map");
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