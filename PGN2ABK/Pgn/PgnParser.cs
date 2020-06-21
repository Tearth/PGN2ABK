using System.Collections.Generic;
using System.Linq;
using PGN2ABK.Board;

namespace PGN2ABK.Pgn
{
    public class PgnParser
    {
        private PgnGameParser _gameParser;

        public PgnParser()
        {
            _gameParser = new PgnGameParser();
        }

        public IEnumerable<IntermediateEntry> Parse(IEnumerable<string> input)
        {
            IntermediateEntry root = new IntermediateEntry(Move.Zero);
            IntermediateEntry currentNode = root;
            foreach (var line in input.Where(p => p.StartsWith('1')))
            {
                var pgnEntry = _gameParser.Parse(line);
                foreach (var move in pgnEntry.Moves)
                {
                    if (!currentNode.Children.Any(p => p.Move == move))
                    {
                        var entry = new IntermediateEntry(move);
                        switch (pgnEntry.GameResult)
                        {
                            case GameResult.WhiteWon:
                            {
                                entry.WhiteWins++;
                                break;
                            }

                            case GameResult.BlackWon:
                            {
                                entry.BlackWins++;
                                break;
                            }
                        }

                        currentNode.Children.Add(entry);
                        currentNode = entry;
                    }
                    else
                    {
                        var existingEntry = currentNode.Children.First(p => p.Move == move);
                        switch (pgnEntry.GameResult)
                        {
                            case GameResult.WhiteWon:
                            {
                                existingEntry.WhiteWins++;
                                break;
                            }

                            case GameResult.BlackWon:
                            {
                                existingEntry.BlackWins++;
                                break;
                            }
                        }

                        currentNode = existingEntry;
                    }
                }

                currentNode = root;
            }

            return null;
        }
    }
}
