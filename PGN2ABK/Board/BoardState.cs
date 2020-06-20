namespace PGN2ABK.Board
{
    public class BoardState
    {
        private PieceType[,] _state;

        public BoardState()
        {
            _state = new [,]
            {
                {PieceType.BRook, PieceType.BKnight, PieceType.BBishop, PieceType.BQueen, PieceType.BKing, PieceType.BBishop, PieceType.BKnight, PieceType.BRook},
                {PieceType.BPawn, PieceType.BPawn,   PieceType.BPawn,   PieceType.BPawn,  PieceType.BPawn, PieceType.BPawn,   PieceType.BPawn,   PieceType.BPawn},
                {PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None,   PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None},
                {PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None,   PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None},
                {PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None,   PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None},
                {PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None,   PieceType.None,  PieceType.None,    PieceType.None,    PieceType.None},
                {PieceType.WPawn, PieceType.WPawn,   PieceType.WPawn,   PieceType.WPawn,  PieceType.WPawn, PieceType.WPawn,   PieceType.WPawn,   PieceType.WPawn},
                {PieceType.WRook, PieceType.WKnight, PieceType.WBishop, PieceType.WQueen, PieceType.WKing, PieceType.WBishop, PieceType.WKnight, PieceType.WRook},
            };
        }

        public PieceType GetPiece(int x, int y)
        {
            return _state[x, y];
        }

        public void SetPiece(int x, int y, PieceType piece)
        {
            _state[x, y] = piece;
        }

        public Move ParseMove(string move)
        {
            return null;
        }
    }
}
