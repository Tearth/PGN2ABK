namespace PGN2ABK.Board
{
    public struct Move
    {
        public Position From { get; set; }
        public Position To { get; set; }
        public int Priority { get; set; }

        public Move(Position from, Position to, int priority)
        {
            From = from;
            To = to;
            Priority = priority;
        }
    }
}
