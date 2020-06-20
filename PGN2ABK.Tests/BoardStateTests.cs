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
    }
}
