using CommandLine;

namespace PGN2ABK.CommandLine
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "PGN input file to process.")]
        public string Input { get; set; }

        [Option('o', "output", Required = true, HelpText = "ABK output file.")]
        public string Output { get; set; }

        [Option('p', "plies", Required = false, HelpText = "Maximal number of plies to parse.")]
        public int PliesCount { get; set; }

        [Option('e', "elo", Required = false, HelpText = "Minimal average ELO of players to parse the game.")]
        public int MinElo { get; set; }

        [Option('t', "time", Required = false, HelpText = "Minimal initial time to parse the game.")]
        public int MinMainTime { get; set; }

        [Option('m', "multithreading", Required = false, HelpText = "Enable support for multithreading.")]
        public bool Multithreading { get; set; }
    }
}
