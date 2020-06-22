using System;

namespace PGN2ABK.Board
{
    [Flags]
    public enum MoveFlags : byte
    {
        None = 0,
        ShortCastling = 1,
        LongCastling = 2,
        EnPassant = 4,
        Promotion = 8
    }
}
