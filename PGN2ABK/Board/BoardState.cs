using System;
using System.Text;
using System.Text.RegularExpressions;
using PGN2ABK.Helpers;

namespace PGN2ABK.Board
{
    public class BoardState
    {
        private readonly PieceType[,] _state;

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

        public Position FindPiece(PieceType piece)
        {
            for (var x = 1; x <= 8; x++)
            {
                for (var y = 1; y <= 8; y++)
                {
                    if (GetPiece(x, y) == piece)
                    {
                        return new Position(x, y);
                    }
                }
            }

            return Position.Zero;
        }

        public Move ParseMove(string move, bool white)
        {
            move = RemoveUnusedCharsFromMove(move);
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
                    return new Move(kingPosition, kingPosition + new Position(2, 0), MoveFlags.ShortCastling);
                }

                // Piece move
                case 3:
                {
                    return ParsePieceMove(move, white, false, false);
                }

                // Promotion
                case 4 when move[2] == '=':
                {
                    return ParsePromotion(move, white, false);
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

                // Piece move from the specified file
                case 4 when move[1] != 'x':
                {
                    return ParsePieceMove(move, white, false, true);
                }

                // Long castling
                case 5 when move == "O-O-O":
                {
                    var kingPosition = white ? new Position(5, 1) : new Position(5, 8);
                    return new Move(kingPosition, kingPosition - new Position(2, 0), MoveFlags.LongCastling);
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

                // Promotion with kill
                case 6 when move[1] == 'x' && move[4] == '=':
                {
                    return ParsePromotion(move, white, true);
                }

                // Piece kill from the specified rank
                case 6 when move[3] == 'x':
                {
                    return ParsePieceMove(move, white, true, true);
                }
            }

            throw new ArgumentException($"Can't parse \"{move}\"", nameof(move));
        }

        public void ExecuteMove(Move move)
        {
            var pieceType = GetPiece(move.From);
            var shortCastling = (move.Flags & MoveFlags.ShortCastling) != 0;
            var longCastling = (move.Flags & MoveFlags.LongCastling) != 0;
            var enPassant = (move.Flags & MoveFlags.EnPassant) != 0;
            var promotion = (move.Flags & MoveFlags.Promotion) != 0;

            if (shortCastling || longCastling)
            {
                var castlePosition = shortCastling ? new Position((sbyte)8, move.From.Y) : new Position((sbyte)1, move.From.Y);
                var castleOffset = shortCastling ? new Position(-2, 0) : new Position(3, 0);
                var castleTargetPosition = castlePosition + castleOffset;
                var castlePieceType = GetPiece(castlePosition);

                SetPiece(castlePosition, PieceType.None);
                SetPiece(castleTargetPosition, castlePieceType);
            }
            
            if (enPassant)
            {
                SetPiece(new Position(move.To.X, move.From.Y), PieceType.None);
            }

            SetPiece(move.From, PieceType.None);
            SetPiece(move.To, promotion ? move.PromotionPiece : pieceType);
        }

        public string GetDebugVisualization()
        {
            var stringBuilder = new StringBuilder();
            for (var y = 7; y >= 0; y--)
            {
                for (var x = 0; x < 8; x++)
                {
                    if (_state[y, x] == PieceType.None)
                    {
                        stringBuilder.Append(". ");
                    }
                    else
                    {
                        stringBuilder.Append(PieceConverter.ToSymbol(_state[y, x]));
                        stringBuilder.Append(" ");
                    }
                }

                stringBuilder.Append("\r\n");
            }

            return stringBuilder.ToString();
        }

        private unsafe string RemoveUnusedCharsFromMove(string move)
        {
            // Unsafe version is way faster than string.Replace and Regex.Replace
            var length = move.Length;
            char* newChars = stackalloc char[length];
            char* currentChar = newChars;

            for (var i = 0; i < length; ++i)
            {
                var c = move[i];
                switch (c)
                {
                    case '#':
                    case '+':
                    case '?':
                    case '!':
                    {
                        continue;
                    }
                    default:
                    {
                        *currentChar++ = c;
                        break;
                    }
                }
            }

            return new string(newChars, 0, (int)(currentChar - newChars));

            // Screw string.Replace, it litters my memory as hell
            // return move.Replace("#", "").Replace("+", "").Replace("?", "").Replace("!", "");
            
            // Screw Regex, it's slow as hell
            // return Regex.Replace(move, @"[\#\|\+|\?|\!]", "");
        }

        private Move ParsePawnMove(string move, bool white, bool kill)
        {
            // exd4 when kill, otherwise e4
            var targetMove = kill ? move.Substring(2, 2) : move;
            var targetPosition = PositionConverter.FromPgn(targetMove);
            var targetPiece = white ? PieceType.WPawn : PieceType.BPawn;
            var sign = white ? 1 : -1;

            var relativeFile = kill ? targetMove[0] > move[0] ? 1 : -1 : 0;
            var shortPushFrom = targetPosition - new Position(relativeFile, 1 * sign);
            var longPushFrom = targetPosition - new Position(relativeFile, 2 * sign);
            var flags = MoveFlags.None;

            if (kill && GetPiece(targetPosition) == PieceType.None)
            {
                // Check en passant
                var enemyPawn = white ? PieceType.BPawn : PieceType.WPawn;
                var enemyPawnPosition = targetPosition - new Position(0, sign);

                if (GetPiece(enemyPawnPosition) != enemyPawn)
                {
                    throw new ArgumentException($"Kill move \"{move}\" on an empty field", nameof(move));
                }

                flags = MoveFlags.EnPassant;
            }

            if (!kill && GetPiece(targetPosition) != PieceType.None)
            {
                throw new ArgumentException($"Pawn move \"{move}\" on an non empty field", nameof(move));
            }

            if (GetPiece(shortPushFrom) == targetPiece)
            {
                return new Move(shortPushFrom, targetPosition, flags);
            }
            else if (GetPiece(longPushFrom) == targetPiece)
            {
                return new Move(longPushFrom, targetPosition, flags);
            }

            throw new ArgumentException($"Can't parse \"{move}\" (pawn push)", nameof(move));
        }
        
        private Move ParsePieceMove(string move, bool white, bool kill, bool ambiguity)
        {
            var pieceType = PieceConverter.FromPgn(move[0], white);
            var targetPosition = PositionConverter.FromPgn(move.Substring(move.Length - 2, 2));
            var sourcePosition = GetSourcePosition(move, targetPosition, pieceType, white, ambiguity);

            if (kill && GetPiece(sourcePosition) == PieceType.None)
            {
                throw new ArgumentException($"Kill move \"{move}\" on an empty field", nameof(move));
            }

            if (!kill && GetPiece(targetPosition) != PieceType.None)
            {
                throw new ArgumentException($"Piece move \"{move}\" on an non empty field", nameof(move));
            }

            return new Move(sourcePosition, targetPosition);
        }

        private Move ParsePromotion(string move, bool white, bool kill)
        {
            if (!kill)
            {
                var targetPosition = PositionConverter.FromPgn(move.Substring(0, 2));
                var sourcePosition = targetPosition - new Position(0, white ? 1 : -1);
                var promotionPiece = PieceConverter.FromPgn(move[3], white);

                return new Move(sourcePosition, targetPosition, MoveFlags.Promotion, promotionPiece);
            }
            else
            {
                var targetPosition = PositionConverter.FromPgn(move.Substring(2, 2));
                var sourcePosition = new Position(move[0] - 'a' + 1, white ? 7 : 2);
                var promotionPiece = PieceConverter.FromPgn(move[5], white);

                return new Move(sourcePosition, targetPosition, MoveFlags.Promotion, promotionPiece);
            }
        }

        private Position GetSourcePosition(string move, Position targetPosition, PieceType piece, bool white, bool ambiguity)
        {
            if (ambiguity)
            {
                // Rank ambiguity, Rg6g7
                if (char.IsDigit(move[2]) && char.IsDigit(move[^1]))
                {
                    return PositionConverter.FromPgn(move.Substring(1, 2));
                }
            }

            for (var x = 1; x <= 8; x++)
            {
                for (var y = 1; y <= 8; y++)
                {
                    var sourcePosition = new Position(x, y);

                    if (ambiguity)
                    {
                        // File ambiguity
                        var file = move[1] - 'a' + 1;
                        if (file != x)
                        {
                            continue;
                        }
                    }

                    if (GetPiece(sourcePosition) == piece)
                    {
                        switch (piece)
                        {
                            case PieceType.WKnight:
                            case PieceType.BKnight:
                            {
                                if (CanMoveAsKnight(sourcePosition, targetPosition, white))
                                {
                                    return sourcePosition;
                                }

                                break;
                            }

                            case PieceType.WBishop:
                            case PieceType.BBishop:
                            {
                                if (CanMoveAsBishop(sourcePosition, targetPosition, white))
                                {
                                    return sourcePosition;
                                }

                                break;
                            }

                            case PieceType.WRook:
                            case PieceType.BRook:
                            {
                                if (CanMoveAsRook(sourcePosition, targetPosition, white))
                                {
                                    return sourcePosition;
                                }

                                break;
                            }

                            case PieceType.WQueen:
                            case PieceType.BQueen:
                            {
                                if (CanMoveAsBishop(sourcePosition, targetPosition, white) ||
                                    CanMoveAsRook(sourcePosition, targetPosition, white))
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

        private bool CanMoveAsKnight(Position sourcePosition, Position targetPosition, bool white)
        {
            var delta = (sourcePosition - targetPosition).Abs();
            if (delta.X == 2 && delta.Y == 1 || delta.X == 1 && delta.Y == 2)
            {
                return !IsPiecePinned(sourcePosition, targetPosition, white);
            }

            return false;
        }

        private bool CanMoveAsBishop(Position sourcePosition, Position targetPosition, bool white)
        {
            var delta = (sourcePosition - targetPosition).Abs();
            if (delta.X == delta.Y)
            {
                return !IsPieceBetween(sourcePosition, targetPosition) && !IsPiecePinned(sourcePosition, targetPosition, white);
            }

            return false;
        }

        private bool CanMoveAsRook(Position sourcePosition, Position targetPosition, bool white)
        {
            if (sourcePosition.X == targetPosition.X && sourcePosition.Y != targetPosition.Y ||
                sourcePosition.X != targetPosition.X && sourcePosition.Y == targetPosition.Y)
            {
                if (!IsPieceBetween(sourcePosition, targetPosition) && !IsPiecePinned(sourcePosition, targetPosition, white))
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

        private bool IsPiecePinned(Position sourcePosition, Position targetPosition, bool white)
        {
            var kingPosition = FindPiece(white ? PieceType.WKing : PieceType.BKing);
            var sourceDelta = kingPosition - sourcePosition;
            var sourceDeltaAbs = sourceDelta.Abs();

            // Check if king is not on the same rank/file or diagonal as the piece
            if (sourceDeltaAbs.X != sourceDeltaAbs.Y && sourceDelta.X != 0 && sourceDelta.Y != 0)
            {
                return false;
            }

            var stepX = sourcePosition.X == kingPosition.X ? 0 : kingPosition.X > sourcePosition.X ? -1 : 1;
            var stepY = sourcePosition.Y == kingPosition.Y ? 0 : kingPosition.Y > sourcePosition.Y ? -1 : 1;
            var step = new Position(stepX, stepY);
            var current = kingPosition + step;

            while (current.IsValid())
            {
                if (current == sourcePosition)
                {
                    current += step;
                    continue;
                }

                if (current == targetPosition)
                {
                    return false;
                }

                var pieceOnField = GetPiece(current);
                if (pieceOnField != PieceType.None)
                {
                    if (step.X != 0 && step.Y != 0)
                    {
                        return white ? 
                            pieceOnField == PieceType.BBishop || pieceOnField == PieceType.BQueen :
                            pieceOnField == PieceType.WBishop || pieceOnField == PieceType.WQueen;
                    }
                    else
                    {
                        return white ?
                            pieceOnField == PieceType.BRook || pieceOnField == PieceType.BQueen :
                            pieceOnField == PieceType.WRook || pieceOnField == PieceType.WQueen;
                    }
                }

                current += step;
            }

            return false;
        }
    }
}
