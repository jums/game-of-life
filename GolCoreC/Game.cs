using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jums.GameOfLife.CoreC
{
    /// <summary>
    /// The game engine of the game of life.
    /// </summary>
    public class Game
    {
        private GrimReaper grimReaper;

        public Game()
        {
            grimReaper = new GrimReaper();
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

        /// <summary>
        /// Seed number for the randomization of the initial life.
        /// </summary>
        public long Seed { get; set; }

        /// <summary>
        /// Initiates the world and fills it with life that will evolve 
        /// tick-by-tick until <c>MaxTics</c> is reached.
        /// </summary>
        public void CreateLife() 
        {
            
        }
    }
}
