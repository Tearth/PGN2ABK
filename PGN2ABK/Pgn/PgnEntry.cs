using System.Collections.Generic;
using PGN2ABK.Board;

namespace PGN2ABK.Pgn
{
    public class PgnEntry
    {
        public bool WhiteWon { get; set; }
        public IEnumerable<Move> Moves { get; set; }

        public PgnEntry(bool whiteWon, IEnumerable<Move> moves)
        {
            WhiteWon = whiteWon;
            Moves = moves;
        }
    }
}
