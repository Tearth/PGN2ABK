namespace PGN2ABK.Board
{
    public class Move
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Priority { get; set; }

        public Move(int from, int to, int priority)
        {
            From = from;
            To = to;
            Priority = priority;
        }
    }
}
