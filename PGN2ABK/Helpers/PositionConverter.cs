using PGN2ABK.Board;

namespace PGN2ABK.Helpers
{
    public static class PositionConverter
    {
        public static Position FromPgn(string pgn)
        {
            var x = pgn[0] - 'a' + 1;
            var y = pgn[1] - '1' + 1;

            return new Position(x, y);
        }
    }
}
