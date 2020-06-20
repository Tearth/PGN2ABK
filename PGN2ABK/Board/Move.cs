namespace PGN2ABK.Board
{
    public struct Move
    {
        public Position From { get; set; }
        public Position To { get; set; }
        public bool ShortCastling { get; set; }
        public bool LongCastling { get; set; }
        public bool EnPassant { get; set; }
        public PieceType? Promotion { get; set; }

        public static Move Zero = new Move();
    }
}
