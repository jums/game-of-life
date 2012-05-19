using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace Jums.GameOfLife.CoreC
{
    class World
    {
        public const int MinimumSize = 10;
        private int fillRate = 50;

        public World(int width, int height)
        {
            if (width < MinimumSize) throw new ArgumentOutOfRangeException("width", "width was less than the minimum");
            if (height < MinimumSize) throw new ArgumentOutOfRangeException("height", "height was less than the minimum");

            this.Width = width;
            this.Height = height;
            this.LifeStates = Enumerable.Repeat(false, width * height).ToList();
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        private List<bool> LifeStates { get; set; }
        
        public int FillRate 
        {
            get 
            { 
                return fillRate; 
            }
            set 
            {
                if (value > 100) fillRate = 100;
                else if (value < 0) fillRate = 0;
                else fillRate = value;
            }
        }

        public bool IsAlive(int x, int y)
        {
            if (x > this.Width - 1) throw new IndexOutOfRangeException("x was beyond the world, positive.");
            if (y > this.Height - 1) throw new IndexOutOfRangeException("y was beyond the world, positive.");
            if (x < 0) throw new IndexOutOfRangeException("x was beyond the world, negative.");
            if (y < 0) throw new IndexOutOfRangeException("y was beyond the world, negative.");

            int index = GetPositionIndex(x, y);
            return this.LifeStates[index];
        }

        internal int GetPositionIndex(int x, int y)
        {
            return y * this.Width + x;
        }

        internal void CreateLife()
        {
            // TODO
            this.LifeStates[0] = true;
        }
    }
}
