using System;
using System.Collections.Generic;
using System.Linq;
using PGN2ABK.Board;

namespace PGN2ABK.Pgn
{
    public class PgnParser
    {
        private readonly PgnGameParser _gameParser;

        public PgnParser()
        {
            _gameParser = new PgnGameParser();
        }

        public IEnumerable<IntermediateEntry> Parse(IEnumerable<string> input, int maxPlies, int minElo)
        {
            var root = new IntermediateEntry(Move.Zero);
            var currentNode = root;
            var whiteElo = 0;
            var blackElo = 0;

            foreach (var line in input)
            {
                if (line.StartsWith("[WhiteElo"))
                {
                    whiteElo = GetElo(line);
                }
                else if (line.StartsWith("[BlackElo"))
                {
                    blackElo = GetElo(line);
                }
                else if (line.StartsWith('1'))
                {
                    if ((whiteElo + blackElo) / 2 >= minElo)
                    {
                        var pgnEntry = _gameParser.Parse(line, maxPlies);
                        AttachMoves(currentNode, pgnEntry);

                        // Reset root entry
                        currentNode = root;
                    }
                }
            }

            return root.Children;
        }

        private int GetElo(string line)
        {
            var firstQuote = line.IndexOf('"');
            var lastQuote = line.IndexOf('"', firstQuote + 1);
            var value = line.Substring(firstQuote + 1, lastQuote - firstQuote - 1);

            if (!int.TryParse(line, out var result))
            {
                return 0;
            }

            return result;
        }

        private void AttachMoves(IntermediateEntry current, PgnEntry pgnEntry)
        {
            foreach (var move in pgnEntry.Moves)
            {
                if (current.Children.All(p => p.Move != move))
                {
                    AddNewIntermediateEntry(ref current, move, pgnEntry.GameResult);
                }
                else
                {
                    UpdateIntermediateEntry(ref current, move, pgnEntry.GameResult);
                }
            }
        }

        private void AddNewIntermediateEntry(ref IntermediateEntry current, Move move, GameResult result)
        {
            var entry = new IntermediateEntry(move);
            entry.IncrementStats(result);

            current.Children.Add(entry);
            current = entry;
        }

        private void UpdateIntermediateEntry(ref IntermediateEntry current, Move move, GameResult result)
        {
            var existingEntry = current.Children.First(p => p.Move == move);
            existingEntry.IncrementStats(result);

            current = existingEntry;
        }
    }
}
