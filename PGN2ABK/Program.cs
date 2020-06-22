using System;
using System.Diagnostics;
using System.IO;
using CommandLine;
using PGN2ABK.CommandLine;
using PGN2ABK.Pgn;

namespace PGN2ABK
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(options =>
            {
                var parser = new PgnParser();
                var input = File.ReadLines(options.Input);

                Console.WriteLine("Start");
                var stopWatch = Stopwatch.StartNew();
                var intermediateEntries = parser.Parse(input, options.PliesCount, options.MinElo);
                var elapsed = stopWatch.Elapsed.TotalSeconds;
                Console.WriteLine($"Stop: {elapsed}");

                Console.Read();
            });
        }
    }
}
