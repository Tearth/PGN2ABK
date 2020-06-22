using System.Collections.Generic;
using PGN2ABK.Board;

namespace PGN2ABK.Pgn
{
    public class PgnEntry
    {
        public GameResult GameResult { get; set; }
        public IEnumerable<Move> Moves { get; set; }
    }
}
