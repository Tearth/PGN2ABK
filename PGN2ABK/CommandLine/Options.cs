using CommandLine;

namespace PGN2ABK.CommandLine
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "PGN input file to process.")]
        public string Input { get; set; }

        [Option('o', "output", Required = true, HelpText = "ABK output file.")]
        public string Output { get; set; }
    }
}
