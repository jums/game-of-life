﻿using System;
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
        private World world;

        public GrimReaper(World world) 
        {
            this.world = world;
        }

        public bool IsAlive(int x, int y)
        {
            return false;
        }
    }
}