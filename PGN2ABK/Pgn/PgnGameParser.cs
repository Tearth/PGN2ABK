﻿using System.Collections.Generic;
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

            foreach (var rawMove in moves)
            {
                var parsedMove = board.ParseMove(rawMove);
            }

            return null;
        }

        private IEnumerable<string> SplitGameIntoMoves(string game)
        {
            return game.Split(' ').Where(p => !p.EndsWith('.'));
        }
    }
}
