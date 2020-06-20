using PGN2ABK.Board;
using Xunit;

namespace PGN2ABK.Tests
{
    public class BoardStateTests
    {
        [Fact]
        public void WhitePawnShortPush()
        {
            var board = new BoardState();
            var parsedMove = board.ParseMove("e3", true);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(5, 2), parsedMove.From);
            Assert.Equal(new Position(5, 3), parsedMove.To);
        }

        [Fact]
        public void WhitePawnLongPush()
        {
            var board = new BoardState();
            var parsedMove = board.ParseMove("e4", true);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(5, 2), parsedMove.From);
            Assert.Equal(new Position(5, 4), parsedMove.To);
        }

        [Fact]
        public void BlackPawnShortPush()
        {
            var board = new BoardState();
            var parsedMove = board.ParseMove("e6", false);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(5, 7), parsedMove.From);
            Assert.Equal(new Position(5, 6), parsedMove.To);
        }

        [Fact]
        public void BlackPawnLongPush()
        {
            var board = new BoardState();
            var parsedMove = board.ParseMove("e5", false);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(5, 7), parsedMove.From);
            Assert.Equal(new Position(5, 5), parsedMove.To);
        }

        [Fact]
        public void WhiteKnightMove()
        {
            var board = new BoardState();
            var parsedMove = board.ParseMove("Nc3", true);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(2, 1), parsedMove.From);
            Assert.Equal(new Position(3, 3), parsedMove.To);

            parsedMove = board.ParseMove("Ne4", true);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(3, 3), parsedMove.From);
            Assert.Equal(new Position(5, 4), parsedMove.To);
        }

        [Fact]
        public void BlackKnightMove()
        {
            var board = new BoardState();
            var parsedMove = board.ParseMove("Nc6", false);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(2, 8), parsedMove.From);
            Assert.Equal(new Position(3, 6), parsedMove.To);

            parsedMove = board.ParseMove("Ne5", false);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(3, 6), parsedMove.From);
            Assert.Equal(new Position(5, 5), parsedMove.To);
        }

        [Fact]
        public void WhiteBishopMove()
        {
            var board = new BoardState();
            board.SetPiece(4, 2, PieceType.None);
            board.SetPiece(5, 2, PieceType.None);

            var parsedMove = board.ParseMove("Bf4", true);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(3, 1), parsedMove.From);
            Assert.Equal(new Position(6, 4), parsedMove.To);

            parsedMove = board.ParseMove("Bc4", true);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(6, 1), parsedMove.From);
            Assert.Equal(new Position(3, 4), parsedMove.To);
        }

        [Fact]
        public void BlackBishopMove()
        {
            var board = new BoardState();
            board.SetPiece(4, 7, PieceType.None);
            board.SetPiece(5, 7, PieceType.None);

            var parsedMove = board.ParseMove("Bf5", false);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(3, 8), parsedMove.From);
            Assert.Equal(new Position(6, 5), parsedMove.To);

            parsedMove = board.ParseMove("Bc5", false);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(6, 8), parsedMove.From);
            Assert.Equal(new Position(3, 5), parsedMove.To);
        }

        [Fact]
        public void WhiteRookMove()
        {
            var board = new BoardState();
            board.SetPiece(1, 2, PieceType.None);

            var parsedMove = board.ParseMove("Ra4", true);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(1, 1), parsedMove.From);
            Assert.Equal(new Position(1, 4), parsedMove.To);

            parsedMove = board.ParseMove("Re4", true);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(1, 4), parsedMove.From);
            Assert.Equal(new Position(5, 4), parsedMove.To);
        }

        [Fact]
        public void BlackRookMove()
        {
            var board = new BoardState();
            board.SetPiece(1, 7, PieceType.None);

            var parsedMove = board.ParseMove("Ra5", false);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(1, 8), parsedMove.From);
            Assert.Equal(new Position(1, 5), parsedMove.To);

            parsedMove = board.ParseMove("Re5", false);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(1, 5), parsedMove.From);
            Assert.Equal(new Position(5, 5), parsedMove.To);
        }

        [Fact]
        public void WhiteQueenMove()
        {
            var board = new BoardState();
            board.SetPiece(3, 2, PieceType.None);

            var parsedMove = board.ParseMove("Qa4", true);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(4, 1), parsedMove.From);
            Assert.Equal(new Position(1, 4), parsedMove.To);

            parsedMove = board.ParseMove("Qh4", true);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(1, 4), parsedMove.From);
            Assert.Equal(new Position(8, 4), parsedMove.To);
        }

        [Fact]
        public void BlackQueenMove()
        {
            var board = new BoardState();
            board.SetPiece(3, 2, PieceType.None);

            var parsedMove = board.ParseMove("Qa5", false);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(4, 8), parsedMove.From);
            Assert.Equal(new Position(1, 5), parsedMove.To);

            parsedMove = board.ParseMove("Qh5", false);
            board.ExecuteMove(parsedMove);

            Assert.Equal(new Position(1, 5), parsedMove.From);
            Assert.Equal(new Position(8, 5), parsedMove.To);
        }
    }
}
