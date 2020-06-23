using PGN2ABK.Helpers;
using PGN2ABK.Pgn;

namespace PGN2ABK.Abk
{
    public readonly struct AbkEntry
    {
        public byte From { get; }
        public byte To { get; }
        public byte Promotion { get; }
        public byte Priority { get; }
        public int Games { get; }
        public int Won { get; }
        public int Lost { get; }
        public int PlyCount { get; }
        public int NextMove { get; }
        public int NextSibling { get; }

        public AbkEntry(IntermediateEntry intermediateEntry, int nextMove, int nextSibling)
        {
            From = intermediateEntry.Move.From.ToIndex();
            To = intermediateEntry.Move.To.ToIndex();
            Promotion = PieceConverter.ToIndex(intermediateEntry.Move.PromotionPiece);
            Priority = 5;
            Games = intermediateEntry.TotalGames;
            Won = intermediateEntry.WhiteWins;
            Lost = intermediateEntry.BlackWins;
            PlyCount = intermediateEntry.Ply;
            NextMove = nextMove;
            NextSibling = nextSibling;
        }
    }
}
