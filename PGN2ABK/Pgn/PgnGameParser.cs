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
            var gameResult = GetGameResult(moves[^1]);

            for (var i = 0; i < moves.Count - 1; i++)
            {
                if (moves[i] == "{")
                {
                    skip = true;
                    continue;
                }

                if (moves[i] == "}")
                {
                    skip = false;
                    continue;
                }

                if (skip)
                {
                    continue;
                }

                var parsedMove = board.ParseMove(moves[i], white);
                board.ExecuteMove(parsedMove);
                parsedMoves.Add(parsedMove);

                white = !white;
            }

            return new PgnEntry
            {
                Moves = parsedMoves,
                GameResult = gameResult
            };
        }

        private List<string> SplitGameIntoMoves(string game)
        {
            return game
                .Split(' ')
                .Where(p => !p.EndsWith('.'))
                .ToList();
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
