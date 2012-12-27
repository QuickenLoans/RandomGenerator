using System;
using System.Linq;
using Microsoft.FSharp.Collections;

namespace RandomGenerator.PerfConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // This console app exists solely for doing performance tests against Lib and Interop
            Console.WriteLine("Number of strings to generate:");
            var count = int.Parse(Console.ReadLine());
            TimeMultiple(count);
        }

        private static void TimeMultiple(int count)
        {
            var generator = new RandomGenerator.Interop.Generator();
            var stopwatch = new System.Diagnostics.Stopwatch();

            Console.WriteLine(string.Format("Starting Lib Multiple Generation of {0} strings", count));
            stopwatch.Start();
            var stringList = RandomGenerator.Lib.GenerateMultiple(count, 7, Lib.CharacterTypes.NewCharSet(Lib.CharacterTypes.NewChars(ListModule.OfSeq("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray())), Lib.CharacterTypes.NewChars(ListModule.OfSeq("0123456789".ToArray()))));
            Console.WriteLine(stopwatch.Elapsed);
            stopwatch.Reset();

            Console.WriteLine(string.Format("Starting Interop Multiple Generation of {0} strings", count));
            stopwatch.Start();
            var strings = generator.Multiple(count, 7);
            Console.WriteLine(stopwatch.Elapsed);
            stopwatch.Reset();

            Console.WriteLine(string.Format("Starting Lib Iterative Generation of {0} strings", count));
            stopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                RandomGenerator.Lib.Generate(7, Lib.CharacterTypes.NewCharSet(Lib.CharacterTypes.NewChars(ListModule.OfSeq("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray())), Lib.CharacterTypes.NewChars(ListModule.OfSeq("0123456789".ToArray()))));
            }
            Console.WriteLine(stopwatch.Elapsed);
            stopwatch.Reset();

            Console.ReadLine();
        }
    }
}
