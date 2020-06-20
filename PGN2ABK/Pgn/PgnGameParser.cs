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

                white = !white;
            }

            return null;
        }

        private IEnumerable<string> SplitGameIntoMoves(string game)
        {
            return game.Split(' ').Where(p => !p.EndsWith('.'));
        }
    }
}
