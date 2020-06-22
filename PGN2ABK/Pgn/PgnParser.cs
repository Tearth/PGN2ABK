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

        public IEnumerable<IntermediateEntry> Parse(IEnumerable<string> input)
        {
            var root = new IntermediateEntry(Move.Zero);
            var currentNode = root;

            foreach (var line in input.Where(p => p.StartsWith('1')))
            {
                var pgnEntry = _gameParser.Parse(line);
                AttachMoves(currentNode, pgnEntry);

                // Reset root entry
                currentNode = root;
            }

            return null;
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
