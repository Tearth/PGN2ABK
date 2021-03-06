﻿using System;
using PGN2ABK.Board;

namespace PGN2ABK.Helpers
{
    public static class PieceConverter
    {
        public static PieceType FromSymbol(char symbol, bool white)
        {
            switch (symbol)
            {
                case 'N': return white ? PieceType.WKnight : PieceType.BKnight;
                case 'B': return white ? PieceType.WBishop : PieceType.BBishop;
                case 'R': return white ? PieceType.WRook : PieceType.BRook;
                case 'Q': return white ? PieceType.WQueen : PieceType.BQueen;
                case 'K': return white ? PieceType.WKing : PieceType.BKing;
            }

            throw new ArgumentException($"Unrecognized piece symbol: \"{symbol}\"", nameof(symbol));
        }

        public static char ToSymbol(PieceType type)
        {
            switch (type)
            {
                case PieceType.WPawn: return 'p';
                case PieceType.WKnight: return 'n';
                case PieceType.WBishop: return 'b';
                case PieceType.WRook: return 'r';
                case PieceType.WQueen: return 'q';
                case PieceType.WKing: return 'k';

                case PieceType.BPawn: return 'P';
                case PieceType.BKnight: return 'N';
                case PieceType.BBishop: return 'B';
                case PieceType.BRook: return 'R';
                case PieceType.BQueen: return 'Q';
                case PieceType.BKing: return 'K';
            }

            throw new ArgumentException($"Unrecognized piece type: \"{type}\"", nameof(type));
        }

        public static byte ToIndex(PieceType pieceType)
        {
            switch (pieceType)
            {
                case PieceType.None:
                {
                    return 0;
                }

                case PieceType.WRook:
                case PieceType.BRook:
                {
                    return 1;
                }

                case PieceType.WKnight:
                case PieceType.BKnight:
                {
                    return 2;
                }

                case PieceType.WBishop:
                case PieceType.BBishop:
                {
                    return 3;
                }

                case PieceType.WQueen:
                case PieceType.BQueen:
                {
                    return 4;
                }
            }

            throw new ArgumentException($"Invalid promotion piece type: \"{pieceType}\"", nameof(pieceType));
        }
    }
}
