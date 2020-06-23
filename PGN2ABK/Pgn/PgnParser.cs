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
        private object _attachLock = new object();

        public PgnParser()
        {
            _gameParser = new PgnGameParser();
        }

        public IEnumerable<IntermediateEntry> Parse(IEnumerable<string> input, int maxPlies, int minElo, bool multithreading)
        {
            var root = new IntermediateEntry(Move.Zero, -1);
            var currentNode = root;
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
                            ThreadPool.QueueUserWorkItem(_ => ParseMoves(ref currentNode, root, line, maxPlies));
                        }
                        else
                        {
                            ParseMoves(ref currentNode, root, line, maxPlies);
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

            if (!int.TryParse(line, out var result))
            {
                return 0;
            }

            return result;
        }

        private void ParseMoves(ref IntermediateEntry currentNode, IntermediateEntry root, string line, int maxPlies)
        {
            var pgnEntry = _gameParser.Parse(line, maxPlies);
            _parsedGames++;

            AttachMoves(currentNode, pgnEntry);

            // Reset root entry
            currentNode = root;

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
                        AddNewIntermediateEntry(ref current, move, ply, pgnEntry.GameResult);
                    }
                    else
                    {
                        UpdateIntermediateEntry(ref current, move, pgnEntry.GameResult);
                    }

                    _parsedMoves++;
                    ply++;
                }
            }
        }

        private void AddNewIntermediateEntry(ref IntermediateEntry current, Move move, int ply, GameResult result)
        {
            var entry = new IntermediateEntry(move, ply);
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
