using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jums.GameOfLife.CoreC
{
    /// <summary>
    /// Master of life and death. He giveth, and taketh away life according to 
    /// the rules of the game of life.
    /// </summary>
    class GrimReaper
    {
        public GrimReaper() 
        {
        }

        public bool IsAlive(World world, int x, int y)
        {
            if (world == null) throw new ArgumentNullException("world");

            return false;
        }
    }
}
