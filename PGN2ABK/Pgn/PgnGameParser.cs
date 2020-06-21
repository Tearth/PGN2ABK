using System.Collections.Generic;
using System.Linq;
using PGN2ABK.Board;

namespace PGN2ABK.Pgn
{
    public class PgnGameParser
    {
        public PgnEntry Parse(string game)
        {
            var board = new BoardState();
            var moves = SplitGameIntoMoves(game);
            var white = true;
            var skip = false;

            var parsedMoves = new List<Move>();
            foreach (var rawMove in moves.Take(moves.Count() - 1))
            {
                if (rawMove == "{")
                {
                    skip = true;
                    continue;
                }

                if (rawMove == "}")
                {
                    skip = false;
                    continue;
                }

                if (skip)
                {
                    continue;
                }

                var parsedMove = board.ParseMove(rawMove, white);
                board.ExecuteMove(parsedMove);
                parsedMoves.Add(parsedMove);

                white = !white;
            }

            return new PgnEntry(GetGameResult(moves.Last()), parsedMoves);
        }

        private IEnumerable<string> SplitGameIntoMoves(string game)
        {
            return game.Split(' ').Where(p => !p.EndsWith('.'));
        }

        private GameResult GetGameResult(string result)
        {
            switch (result)
            {
                case "1-0": return GameResult.WhiteWon;
                case "0-1": return GameResult.BlackWon;
                case "1/2-1/2": return GameResult.Draw;
            }

            return GameResult.None;
        }
    }
}
