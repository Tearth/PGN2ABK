using System;
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
            move = move.Replace("#", "");
            move = move.Replace("+", "");

            switch (move.Length)
            {
                // Pawn move
                case 2:
                {
                    return ParsePawnMove(move, white, false);
                }

                // Short castling
                case 3 when move == "O-O":
                {
                    var kingPosition = white ? new Position(5, 1) : new Position(5, 8);
                    return new Move(kingPosition, kingPosition + new Position(2, 0), 5);
                }

                // Piece move
                case 3:
                {
                    return ParsePieceMove(move, white, false, false);
                }

                // Piece move from the specified file
                case 4 when move[1] != 'x':
                {
                    return ParsePieceMove(move, white, false, true);
                }

                // Pawn kill
                case 4 when char.IsLower(move[0]) && move[1] == 'x':
                {
                    return ParsePawnMove(move, white, true);
                }

                // Piece kill
                case 4 when char.IsUpper(move[0]) && move[1] == 'x':
                {
                    return ParsePieceMove(move, white, true, false);
                }

                // Long castling
                case 5 when move == "O-O-O":
                {
                    var kingPosition = white ? new Position(5, 1) : new Position(5, 8);
                    return new Move(kingPosition, kingPosition - new Position(2, 0), 5);
                }

                // Piece kill from the specified line
                case 5 when move[2] == 'x':
                {
                    return ParsePieceMove(move, white, true, true);
                }

                // Piece move from the specified rank
                case 5 when move[2] != 'x':
                {
                    return ParsePieceMove(move, white, false, true);
                }
            }

            throw new ArgumentException($"Can't parse \"{move}\"", nameof(move));
        }

        public void ExecuteMove(Move move)
        {
            var pieceType = GetPiece(move.From);
            if (pieceType == PieceType.WKing || pieceType == PieceType.BKing)
            {
                if (Math.Abs(move.From.X - move.To.X) == 2)
                {
                    var castlePosition = new Position(move.From.X < move.To.X ? 8 : 1, move.From.Y);
                    var castleTargetPosition = castlePosition + new Position(move.From.X < move.To.X ? -2 : 3, 0);
                    var castlePieceType = GetPiece(castlePosition);

                    SetPiece(castlePosition, PieceType.None);
                    SetPiece(castleTargetPosition, castlePieceType);
                }
            }

            SetPiece(move.From, PieceType.None);
            SetPiece(move.To, pieceType);
        }

        private Move ParsePawnMove(string move, bool white, bool kill)
        {
            var targetMove = kill ? move.Substring(2, 2) : move;
            var targetPosition = PositionConverter.FromPgn(targetMove);
            var targetPiece = white ? PieceType.WPawn : PieceType.BPawn;
            var sign = white ? 1 : -1;

            var relativeFile = kill ? targetMove[0] > move[0] ? 1 : -1 : 0;
            var shortPushFrom = targetPosition - new Position(relativeFile, 1 * sign);
            var longPushFrom = targetPosition - new Position(relativeFile, 2 * sign);

            if (kill && GetPiece(targetPosition) == PieceType.None)
            {
                throw new ArgumentException("Kill move \"{move}\" on an empty field", nameof(move));
            }

            if (!kill && GetPiece(targetPosition) != PieceType.None)
            {
                throw new ArgumentException("Pawn move \"{move}\" on an non empty field", nameof(move));
            }

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
        
        private Move ParsePieceMove(string move, bool white, bool kill, bool ambiguity)
        {
            var pieceType = PieceConverter.FromPgn(move[0], white);
            var targetPosition = PositionConverter.FromPgn(move.Substring(move.Length - 2, 2));
            var sourcePosition = GetSourcePosition(move, targetPosition, pieceType, ambiguity);

            if (kill && GetPiece(sourcePosition) == PieceType.None)
            {
                throw new ArgumentException("Kill move \"{move}\" on an empty field", nameof(move));
            }

            if (!kill && GetPiece(targetPosition) != PieceType.None)
            {
                throw new ArgumentException("Pawn move \"{move}\" on an non empty field", nameof(move));
            }

            return new Move(sourcePosition, targetPosition, 5);
        }

        private Position GetSourcePosition(string move, Position targetPosition, PieceType piece, bool ambiguity)
        {
            for (var x = 1; x <= 8; x++)
            {
                for (var y = 1; y <= 8; y++)
                {
                    var sourcePosition = new Position(x, y);

                    if (ambiguity)
                    {
                        if (move[1] == move[3] && move[2] != 'x')
                        {
                            var rank = move[2] - '0';
                            if (rank != y)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            var file = move[1] - 'a' + 1;
                            if (file != x)
                            {
                                continue;
                            }
                        }
                    }

                    if (GetPiece(sourcePosition) == piece)
                    {
                        switch (piece)
                        {
                            case PieceType.WKnight:
                            case PieceType.BKnight:
                            {
                                if (CanMoveAsKnight(sourcePosition, targetPosition))
                                {
                                    return sourcePosition;
                                }

                                break;
                            }

                            case PieceType.WBishop:
                            case PieceType.BBishop:
                            {
                                if (CanMoveAsBishop(sourcePosition, targetPosition))
                                {
                                    return sourcePosition;
                                }

                                break;
                            }

                            case PieceType.WRook:
                            case PieceType.BRook:
                            {
                                if (CanMoveAsRook(sourcePosition, targetPosition))
                                {
                                    return sourcePosition;
                                }

                                break;
                            }

                            case PieceType.WQueen:
                            case PieceType.BQueen:
                            {
                                if (CanMoveAsBishop(sourcePosition, targetPosition) || CanMoveAsRook(sourcePosition, targetPosition))
                                {
                                    return sourcePosition;
                                }

                                break;
                            }

                            case PieceType.WKing:
                            case PieceType.BKing:
                            {
                                return sourcePosition;
                            }
                        }
                    }
                }
            }

            throw new ArgumentException($"Can't parse \"{move}\" (piece move)", nameof(move));
        }

        private bool CanMoveAsKnight(Position sourcePosition, Position targetPosition)
        {
            var delta = (sourcePosition - targetPosition).Abs();
            return delta.X == 2 && delta.Y == 1 || delta.X == 1 && delta.Y == 2;
        }

        private bool CanMoveAsBishop(Position sourcePosition, Position targetPosition)
        {
            var delta = (sourcePosition - targetPosition).Abs();
            return delta.X == delta.Y;
        }

        private bool CanMoveAsRook(Position sourcePosition, Position targetPosition)
        {
            if (sourcePosition.X == targetPosition.X && sourcePosition.Y != targetPosition.Y ||
                sourcePosition.X != targetPosition.X && sourcePosition.Y == targetPosition.Y)
            {
                if (!IsPieceBetween(sourcePosition, targetPosition))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsPieceBetween(Position a, Position b)
        {
            var stepX = a.X == b.X ? 0 : b.X > a.X ? 1 : -1;
            var stepY = a.Y == b.Y ? 0 : b.Y > a.Y ? 1 : -1;

            var step = new Position(stepX, stepY);
            var current = a + step;

            while (current != b)
            {
                if (GetPiece(current) != PieceType.None)
                {
                    return true;
                }

                current += step;
            }

            return false;
        }
    }
}
