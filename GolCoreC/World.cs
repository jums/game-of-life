using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace Jums.GameOfLife.CoreC
{
    class World
    {
        public World(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        private List<bool> LifeStates { get; set; }
    }
}
