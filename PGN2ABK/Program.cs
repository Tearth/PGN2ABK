using System;
using System.IO;
using CommandLine;
using PGN2ABK.Abk;
using PGN2ABK.CommandLine;
using PGN2ABK.Pgn;
using ShellProgressBar;

namespace PGN2ABK
{
    public class Program
    {
        private static ProgressBar _progressBar;
        private static ulong _inputFileSize;
        private static DateTime _timeFromStart;

        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(options =>
            {
                _inputFileSize = (ulong)new FileInfo(options.Input).Length;

                var progressBarOptions = new ProgressBarOptions
                {
                    ForegroundColor = ConsoleColor.Yellow,
                    ForegroundColorDone = ConsoleColor.DarkGreen,
                    BackgroundColor = ConsoleColor.DarkGray,
                    BackgroundCharacter = '\u2593',
                    ShowEstimatedDuration = true
                };
                _progressBar = new ProgressBar(100, "Initializing...", progressBarOptions);

                var parser = new PgnParser();
                parser.OnStatusUpdate += Parser_OnStatusUpdate;

                var abkGenerator = new AbkGenerator();
                var input = File.ReadLines(options.Input);
                
                _timeFromStart = DateTime.Now;

                var intermediateEntries = parser.Parse(input, options.PliesCount, options.MinElo, options.MinMainTime, options.Multithreading);
                abkGenerator.Save(options.Output, intermediateEntries);

                Console.Read();
            });
        }

        private static void Parser_OnStatusUpdate(object sender, PgnStatusEventArgs args)
        {
            var currentProgress = (int)((float)args.ReadChars / _inputFileSize * 100);
            if (currentProgress > (int) _progressBar.Percentage)
            {
                var message = $"Games: {args.ParsedGames}, Moves: {args.ParsedMoves}";
                var deltaTime = (DateTime.Now - _timeFromStart);
                var estimatedTotalTime = (int)(100 * deltaTime.TotalSeconds / currentProgress);

                _progressBar.Tick(currentProgress, new TimeSpan(0, 0, estimatedTotalTime), message);
            }
        }
    }
}
