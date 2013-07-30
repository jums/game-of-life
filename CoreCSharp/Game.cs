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
        //private World WorldTomorrow { get; set; }
        private MotherNature MotherNature { get; set; }

        public Game()
        {
            MaxTicks = 100;
            InitialWorldWidth = 160;
            InitialWorldHeight = 90;
            Seed = DateTime.Now.Ticks;

            MotherNature = new MotherNature();
            World = new World(InitialWorldWidth, InitialWorldHeight);
        }

        /// <summary>
        /// Maximum number of "ticks", rounds, years, units of time.
        /// Game ends after reaching this tick count at the latest.
        /// </summary>
        public int MaxTicks { get; set; }

        /// <summary>
        /// Initial width of the world.
        /// </summary>
        public int InitialWorldWidth { get; set; }

        /// <summary>
        /// Initial height of the world.
        /// </summary>
        public int InitialWorldHeight { get; set; }

        public int Width
        {
            get { return World.Width; }
        }

        public int Height
        {
            get { return World.Height; }
        }

        /// <summary>
        /// Seed number for the randomization of the initial life.
        /// </summary>
        public long Seed { get; set; }

        /// <summary>
        /// Initiates the world and fills it with life that will evolve 
        /// tick-by-tick until <c>MaxTics</c> is reached.
        /// </summary>
        public void Start() 
        {
            // TODO
        }

        /// <summary>
        /// Goes one time unit forward and evolves the world.
        /// </summary>
        public void Next()
        {
            // TODO
        }

        public bool[,] State
        {
            get { return World.ToArrays(); }
        }

        public void Reset()
        {
            World.CreateLife();
        }
    }
}
