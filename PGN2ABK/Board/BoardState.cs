﻿using System;
using PGN2ABK.Helpers;

namespace PGN2ABK.Board
{
    public class BoardState
    {
        private PieceType[,] _state;

        public BoardState()
        {
            _state = new [,]
            {
                {PieceType.WRook, PieceType.WKnight, PieceType.WBishop, PieceType.WQueen, PieceType.WKing, PieceType.WBishop, PieceType.WKnight, PieceType.WRook},
                {PieceType.WPawn, PieceType.WPawn,   PieceType.WPawn,   PieceType.WPawn,  PieceType.WPawn, PieceType.WPawn,   PieceType.WPawn,   PieceType.WPawn},
                {PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None,   PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None},
                {PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None,   PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None},
                {PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None,   PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None},
                {PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None,   PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None},
                {PieceType.BPawn, PieceType.BPawn,   PieceType.BPawn,   PieceType.BPawn,  PieceType.BPawn, PieceType.BPawn,   PieceType.BPawn,   PieceType.BPawn},
                {PieceType.BRook, PieceType.BKnight, PieceType.BBishop, PieceType.BQueen, PieceType.BKing, PieceType.BBishop, PieceType.BKnight, PieceType.BRook}
            };
        }

        public PieceType GetPiece(Position position)
        {
            return GetPiece(position.X, position.Y);
        }

        public PieceType GetPiece(int x, int y)
        {
            // Board is rotated by 90 degrees, so x and y are inverted
            return _state[y - 1, x - 1];
        }

        public void SetPiece(Position position, PieceType piece)
        {
            SetPiece(position.X, position.Y, piece);
        }

        public void SetPiece(int x, int y, PieceType piece)
        {
            // Board is rotated by 90 degrees, so x and y are inverted
            _state[y - 1, x - 1] = piece;
        }

        public Move ParseMove(string move, bool white)
        {
            switch (move.Length)
            {
                // Pawn push
                case 2:
                {
                    return ParsePawnPush(move, white);
                }

                case 3:
                {
                    return ParsePieceMove(move, white);
                }

                case 4:
                {
                    break;
                }
            }

            throw new ArgumentException($"Can't parse \"{move}\"", nameof(move));
        }

        public void ExecuteMove(Move move)
        {
            var pieceType = GetPiece(move.From);
            SetPiece(move.From, PieceType.None);
            SetPiece(move.To, pieceType);
        }

        private Move ParsePawnPush(string move, bool white)
        {
            var targetPosition = PositionConverter.FromPgn(move);
            var targetPiece = white ? PieceType.WPawn : PieceType.BPawn;
            var sign = white ? 1 : -1;

            var shortPushFrom = targetPosition - new Position(0, 1 * sign);
            var longPushFrom = targetPosition - new Position(0, 2 * sign);

            if (GetPiece(shortPushFrom) == targetPiece)
            {
                return new Move(shortPushFrom, targetPosition, 5);
            }
            else if (GetPiece(longPushFrom) == targetPiece)
            {
                return new Move(longPushFrom, targetPosition, 5);
            }

            throw new ArgumentException($"Can't parse \"{move}\" (pawn push)", nameof(move));
        }
        
        private Move ParsePieceMove(string move, bool white)
        {
            var pieceType = PieceConverter.FromPgn(move[0], white);
            var targetPosition = PositionConverter.FromPgn(move.Substring(1, 2));
            var sourcePosition = GetSourcePosition(move, targetPosition, pieceType);

            return new Move(sourcePosition, targetPosition, 5);
        }

        private Position GetSourcePosition(string move, Position targetPosition, PieceType piece)
        {
            for (var x = 1; x <= 8; x++)
            {
                for (var y = 1; y <= 8; y++)
                {
                    var sourcePosition = new Position(x, y);
                    if (GetPiece(sourcePosition) == piece)
                    {
                        switch (piece)
                        {
                            case PieceType.WKnight:
                            case PieceType.BKnight:
                            {
                                var delta = (sourcePosition - targetPosition).Abs();
                                if (delta.X == 2 && delta.Y == 1 || delta.X == 1 && delta.Y == 2)
                                {
                                    return sourcePosition;
                                }

                                break;
                            }
                        }
                    }
                }
            }

            throw new ArgumentException($"Can't parse \"{move}\" (piece move)", nameof(move));
        }
    }
}