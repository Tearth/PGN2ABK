using System;
using System.Collections.Generic;
using PGN2ABK.Board;

namespace PGN2ABK.Pgn
{
    public class IntermediateEntry
    {
        public Move Move { get; set; }
        public List<IntermediateEntry> Children { get; set; }
        public int WhiteWins { get; set; }
        public int BlackWins { get; set; }
        public int Draws { get; set; }
        public int TotalGames => WhiteWins + BlackWins + Draws;

        public IntermediateEntry(Move move)
        {
            Move = move;
            Children = new List<IntermediateEntry>();
        }

        public void IncrementStats(GameResult result)
        {
            switch (result)
            {
                case GameResult.WhiteWon:
                {
                    WhiteWins++;
                    return;
                }

                case GameResult.BlackWon:
                {
                    BlackWins++;
                    return;
                }

                case GameResult.Draw:
                {
                    Draws++;
                    return;
                }
            }

            throw new ArgumentException("Invalid game result: \"{result\"", nameof(result));
        }
    }
}
