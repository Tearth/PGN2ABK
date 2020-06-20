using System;
using PGN2ABK.Board;

namespace PGN2ABK.Helpers
{
    public static class PieceConverter
    {
        public static PieceType FromPgn(char symbol, bool white)
        {
            switch (symbol)
            {
                case 'N': return white ? PieceType.WKnight : PieceType.BKnight;
                case 'B': return white ? PieceType.WBishop : PieceType.BBishop;
                case 'R': return white ? PieceType.WRook : PieceType.BRook;
                case 'Q': return white ? PieceType.WQueen : PieceType.BQueen;
                case 'K': return white ? PieceType.WKing : PieceType.BKing;
            }

            throw new ArgumentException($"Can't parse \"{symbol}\" piece symbol", nameof(symbol));
        }
    }
}
