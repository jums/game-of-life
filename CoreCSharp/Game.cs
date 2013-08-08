namespace Jums.GameOfLife.CoreCSharp
{
    /// <summary>
    /// The game engine of the game of life.
    /// </summary>
    public class Game
    {
        private World world;
        private readonly Darwin darwin;
        private readonly GreatMaker greatMaker;

        public Game(Settings settings)
        {
            world = new World(settings.Width, settings.Height, settings.Wrapped);
            greatMaker = new GreatMaker(settings.FillRate);
            darwin = new Darwin();
        }

        public Game() : this(new Settings {Height = 45, Width = 80, Wrapped = false})
        {
        }

        /// <summary>
        /// Width of the current world.
        /// </summary>
        public int Width
        {
            get { return world.Width; }
        }

        /// <summary>
        /// Height of the current world.
        /// </summary>
        public int Height
        {
            get { return world.Height; }
        }

        /// <summary>
        /// World life states.
        /// </summary>
        public bool[,] State
        {
            get { return world.To2DArray(); }
        }

        /// <summary>
        /// Goes one time unit forward and evolves the world.
        /// </summary>
        public void Next()
        {
            world = darwin.Evolve(world);
        }

        /// <summary>
        /// Fills the world with new life and clears the old.
        /// </summary>
        /// <param name="seed">The seed for randomizer. Automatically generated if left empty or null.</param>
        /// <returns>The used seed for randomizer.</returns>
        public int Populate(int? seed = null)
        {
            return greatMaker.CreateLife(world, seed);
        }

        /// <summary>
        /// Kills all life on the world.
        /// </summary>
        public void Clear()
        {
            world = world.CopyEmpty();
        }

        /// <summary>
        /// Kills life at given coordinates, if it's there. If not, do nothing.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public void KillLifeAt(int x, int y)
        {
            world.SetLifeAt(x, y, false);
        }

        /// <summary>
        /// Creates life at given coordinates, if it's not there already. If it is, do nothing.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public void CreateLifeAt(int x, int y)
        {
            world.SetLifeAt(x, y, true);
        }
    }
}