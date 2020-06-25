using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PGN2ABK.Board;

namespace PGN2ABK.Pgn
{
    public class PgnParser
    {
        public event EventHandler<PgnStatusEventArgs> OnStatusUpdate;

        private readonly PgnGameParser _gameParser;
        private ulong _parsedGames;
        private ulong _parsedMoves;
        private ulong _readChars;
        private readonly object _attachLock = new object();

        public PgnParser()
        {
            _gameParser = new PgnGameParser();
        }

        public IEnumerable<IntermediateEntry> Parse(IEnumerable<string> input, int maxPlies, int minElo, bool multithreading)
        {
            var root = new IntermediateEntry(Move.Zero, -1);
            var whiteElo = 0;
            var blackElo = 0;

            foreach (var line in input)
            {
                _readChars += (ulong)line.Length + 1;

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
                        if (multithreading)
                        {
                            ThreadPool.QueueUserWorkItem(_ => ParseMoves(root, line, maxPlies));
                        }
                        else
                        {
                            ParseMoves(root, line, maxPlies);
                        }
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

            if (!int.TryParse(value, out var result))
            {
                return 0;
            }

            return result;
        }

        private void ParseMoves(IntermediateEntry root, string line, int maxPlies)
        {
            var pgnEntry = _gameParser.Parse(line, maxPlies);
            _parsedGames++;

            AttachMoves(root, pgnEntry);

            OnStatusUpdate?.Invoke(this, new PgnStatusEventArgs(_parsedGames, _parsedMoves, _readChars));
        }

        private void AttachMoves(IntermediateEntry current, PgnEntry pgnEntry)
        {
            lock (_attachLock)
            {
                var ply = 0;
                foreach (var move in pgnEntry.Moves)
                {
                    if (current.Children.All(p => p.Move != move))
                    {
                        current = AddNewIntermediateEntry(current, move, ply, pgnEntry.GameResult);
                    }
                    else
                    {
                        current = UpdateIntermediateEntry(current, move, pgnEntry.GameResult);
                    }

                    _parsedMoves++;
                    ply++;
                }
            }
        }

        private IntermediateEntry AddNewIntermediateEntry(IntermediateEntry current, Move move, int ply, GameResult result)
        {
            var entry = new IntermediateEntry(move, ply);
            entry.IncrementStats(result);

            current.Children.Add(entry);
            return entry;
        }

        private IntermediateEntry UpdateIntermediateEntry(IntermediateEntry current, Move move, GameResult result)
        {
            var existingEntry = current.Children.First(p => p.Move == move);
            existingEntry.IncrementStats(result);

            return existingEntry;
        }
    }
}
