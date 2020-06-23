using System;

namespace PGN2ABK.Pgn
{
    public class PgnStatusEventArgs : EventArgs
    {
        public ulong ParsedGames { get; }
        public ulong ParsedMoves { get; }
        public ulong ReadChars { get; }

        public PgnStatusEventArgs(ulong parsedGames, ulong parsedMoves, ulong readChars)
        {
            ParsedGames = parsedGames;
            ParsedMoves = parsedMoves;
            ReadChars = readChars;
        }
    }
}
