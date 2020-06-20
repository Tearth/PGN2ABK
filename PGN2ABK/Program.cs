using System;
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

                parser.Parse(input);
                Console.Read();
            });
        }
    }
}
