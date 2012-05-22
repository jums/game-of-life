using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace Jums.GameOfLife.CoreC
{
    /// <summary>
    /// The world is a grid of positions that are dead or alive. 
    /// It doesn't know much else; it just is, flying through space and time
    /// on the back of a giant tortoise.
    /// </summary>
    class World
    {
        public const int MinimumSize = 10;
        private int fillRate = 50;
        private IList<bool> lifeStates;

        public World(int width, int height)
        {
            if (width < MinimumSize) throw new ArgumentOutOfRangeException("width", "width was less than the minimum");
            if (height < MinimumSize) throw new ArgumentOutOfRangeException("height", "height was less than the minimum");

            this.Width = width;
            this.Height = height;
            this.lifeStates = Enumerable.Repeat(false, width * height).ToList();
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// Enumeration of all positions in the world.
        /// </summary>
        public IEnumerable<bool> Positions
        {
            get 
            {
                return new ReadOnlyCollectionBuilder<bool>(lifeStates).ToReadOnlyCollection();
            }
        }

        /// <summary>
        /// The percentage of positions that should contain life.
        /// </summary>
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

        /// <summary>
        /// Determines whether specified coordinates contain life.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>
        /// 	<c>true</c> if the specified coordinates have life; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAlive(int x, int y)
        {
            if (x > this.Width - 1) throw new IndexOutOfRangeException("x was beyond the world, positive.");
            if (y > this.Height - 1) throw new IndexOutOfRangeException("y was beyond the world, positive.");
            if (x < 0) throw new IndexOutOfRangeException("x was beyond the world, negative.");
            if (y < 0) throw new IndexOutOfRangeException("y was beyond the world, negative.");

            int index = GetPositionIndex(x, y);
            return lifeStates[index];
        }

        internal int GetPositionIndex(int x, int y)
        {
            return y * this.Width + x;
        }

        /// <summary>
        /// Fills the world with random life where amount of life versus death is
        /// precisely determined by the <c>FillRate</c>.
        /// </summary>
        public void CreateLife()
        {
            int positions = this.lifeStates.Count;
            int lifeAmount = (int)Math.Round(this.FillRate / 100f * positions);

            List<bool> life = new List<bool>(positions);
            life.AddRange(Enumerable.Repeat(true, lifeAmount));
            life.AddRange(Enumerable.Repeat(false, positions - lifeAmount));

            int seed = (int)(DateTime.Now.Ticks % int.MaxValue + this.GetHashCode());
            Random random = new Random(seed); // TODO: Take seed from property

            for (int i = 0; i < positions; i++) // loop through actual positions
            {
                int lifeIndex = random.Next() % life.Count;
                lifeStates[i] = life[lifeIndex];
                life.RemoveAt(lifeIndex);
            }
        }

        /// <summary>
        /// Fills the world with the specified life data.
        /// </summary>
        /// <param name="data">The life data.</param>
        public void Import(IEnumerable<bool> data)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (data.Count() != lifeStates.Count) throw new ArgumentException("data", "data was bad length");

            bool[] dataArray = data.ToArray();

            for (int i = 0; i < lifeStates.Count; i++)
            {
                lifeStates[i] = dataArray[i];
            }
        }
    }
}
