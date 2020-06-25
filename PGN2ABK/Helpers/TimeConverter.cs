namespace PGN2ABK.Helpers
{
    public static class TimeConverter
    {
        public static (int Main, int Inc) Parse(string value)
        {
            var split = value.Split('+');
            
            if (!int.TryParse(split[0], out var main))
            {
                return (0, 0);
            }

            if (!int.TryParse(split[1], out var inc))
            {
                return (0, 0);
            }

            return (main, inc);
        }
    }
}
