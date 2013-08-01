using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jums.GameOfLife.CoreCSharp
{
    /// <summary>
    /// The game engine of the game of life.
    /// </summary>
    public class Game
    {
        private World World { get; set; }
        private Darwin Darwin { get; set; }

        public Game(Settings settings)
        {
            World = new World(settings.Width, settings.Height, settings.Wrapped);
            Darwin = new Darwin();
        }

        public Game() : this(new Settings {Height = 45, Width = 80, Wrapped = false})
        {
        }

        /// <summary>
        /// Width of the current world.
        /// </summary>
        public int Width
        {
            get { return World.Width; }
        }

        /// <summary>
        /// Height of the current world.
        /// </summary>
        public int Height
        {
            get { return World.Height; }
        }

        public bool[,] State
        {
            get { return World.To2DArray(); }
        }

        /// <summary>
        /// Goes one time unit forward and evolves the world.
        /// </summary>
        public void Next()
        {
            World = Darwin.Evolve(World);
        }

        /// <summary>
        /// Fills the world with new life and clears the old.
        /// </summary>
        /// <param name="seed">The seed for randomizer. Automatically generated if left empty or null.</param>
        /// <returns>The used seed for randomizer.</returns>
        public int Populate(int? seed = null)
        {
            return World.CreateLife(seed);
        }
    }
}