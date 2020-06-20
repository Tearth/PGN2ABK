using System.Collections.Generic;
using System.Linq;

namespace PGN2ABK.Pgn
{
    public class PgnParser
    {
        public IEnumerable<ParserEntry> Parse(IEnumerable<string> input)
        {
            foreach (var line in input.Where(p => p.StartsWith('1')))
            {

            }

            return null;
        }
    }
}
