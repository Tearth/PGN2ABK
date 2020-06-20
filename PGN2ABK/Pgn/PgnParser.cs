using System.Collections.Generic;
using System.Linq;

namespace PGN2ABK.Pgn
{
    public class PgnParser
    {
        private PgnGameParser _gameParser;

        public PgnParser()
        {
            _gameParser = new PgnGameParser();
        }

        public IEnumerable<IntermediateEntry> Parse(IEnumerable<string> input)
        {
            foreach (var line in input.Where(p => p.StartsWith('1')))
            {
                _gameParser.Parse(line);
            }

            return null;
        }
    }
}
