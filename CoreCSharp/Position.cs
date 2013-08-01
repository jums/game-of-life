namespace Jums.GameOfLife.CoreCSharp
{
    /// <summary>
    /// Coordinates to a single position in the world.
    /// </summary>
    internal struct Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;

        public override string ToString()
        {
            return string.Format("X:{0} Y:{1}", X, Y);
        }
    }
}