namespace Jums.GameOfLife.CoreCSharp
{
    internal struct Position
    {
        public int X;
        public int Y;

        public override string ToString()
        {
            return string.Format("X:{0} Y:{1}", X, Y);
        }
    }
}