using System.Collections.Generic;
using PGN2ABK.Board;

namespace PGN2ABK.Pgn
{
    public class IntermediateEntry
    {
        public Move Move { get; set; }
        public List<IntermediateEntry> Children { get; set; }
        public int WhiteWins { get; set; }

        public IntermediateEntry(Move move)
        {
            Move = move;
            Children = new List<IntermediateEntry>();
        }
    }
}
