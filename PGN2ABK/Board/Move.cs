namespace PGN2ABK.Board
{
    public readonly struct Move
    {
        public Position From { get; }
        public Position To { get; }
        public MoveFlags Flags { get; }
        public PieceType PromotionPiece { get; }
        public static Move Zero = new Move();

        public Move(Position from, Position to) : this(from, to, MoveFlags.None, PieceType.None)
        {

        }

        public Move(Position from, Position to, MoveFlags flags) : this(from, to, flags, PieceType.None)
        {

        }

        public Move(Position from, Position to, MoveFlags flags, PieceType promotionPiece)
        {
            From = from;
            To = to;
            Flags = flags;
            PromotionPiece = promotionPiece;
        }

        public static bool operator ==(Move a, Move b)
        {
            return a.From == b.From && a.To == b.To;
        }

        public static bool operator !=(Move a, Move b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            var move = obj as Move?;
            if (move == null)
            {
                return false;
            }

            return this == move;
        }

        public override int GetHashCode()
        {
            return From.GetHashCode() ^ To.GetHashCode() ^ (int)Flags ^ (int)PromotionPiece;
        }
    }
}
