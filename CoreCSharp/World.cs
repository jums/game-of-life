using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace Jums.GameOfLife.CoreCSharp
{
    /// <summary>
    /// The world is a grid of positions that are dead or alive. 
    /// It doesn't know much else; it just is, flying through space and time
    /// on the back of a giant tortoise.
    /// </summary>
    internal class World
    {
        public const int MinimumSize = 10;
        private int fillRate = 10;
        private IList<bool> lifeStates;

        public World(int width, int height)
        {
            if (width < MinimumSize) throw new ArgumentOutOfRangeException("width", string.Format("width was less than the minimum {0}", MinimumSize));
            if (height < MinimumSize) throw new ArgumentOutOfRangeException("height", string.Format("height was less than the minimum {0}", MinimumSize));

            Width = width;
            Height = height;
            lifeStates = Enumerable.Repeat(false, width*height).ToList();
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// Enumeration of all positions in the world.
        /// </summary>
        public IEnumerable<bool> Positions
        {
            get { return new ReadOnlyCollectionBuilder<bool>(lifeStates).ToReadOnlyCollection(); }
        }

        /// <summary>
        /// The percentage of positions that should contain life.
        /// </summary>
        public int FillRate
        {
            get { return fillRate; }
            set
            {
                if (value > 100) fillRate = 100;
                else if (value < 0) fillRate = 0;
                else fillRate = value;
            }
        }

        /// <summary>
        /// Determines whether specified coordinates contain life. Coordinates outside the world are always dead.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>
        /// 	<c>true</c> if the specified coordinates have life; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAlive(int x, int y)
        {
            if (x < 0 || y < 0) return false;
            if (x >= Width || y >= Height) return false;

            int index = GetPositionIndex(x, y);
            return lifeStates[index];
        }

        internal int GetPositionIndex(int x, int y)
        {
            return y*Width + x;
        }

        /// <summary>
        /// Fills the world with random life where amount of life versus death is
        /// precisely determined by the <c>FillRate</c>.
        /// </summary>
        public void CreateLife(int? seed = null)
        {
            int positions = lifeStates.Count;
            int lifeAmount = (int) Math.Round(FillRate/100f*positions);

            List<bool> life = new List<bool>(positions);
            life.AddRange(Enumerable.Repeat(true, lifeAmount));
            life.AddRange(Enumerable.Repeat(false, positions - lifeAmount));

            seed = seed ?? (int) (DateTime.Now.Ticks%Int32.MaxValue + GetHashCode());
            Random random = new Random(seed.Value);

            for (int i = 0; i < positions; i++) // loop through actual positions
            {
                int lifeIndex = random.Next()%life.Count;
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
            if (data.Count() != lifeStates.Count) throw new ArgumentException("data", "data was bad length, " + data.Count());

            bool[] dataArray = data.ToArray();

            for (int i = 0; i < lifeStates.Count; i++)
            {
                lifeStates[i] = dataArray[i];
            }
        }

        /// <summary>
        /// Makes a copy of this object.
        /// </summary>
        /// <returns></returns>
        public World Copy()
        {
            return new World(Width, Height)
            {
                lifeStates = lifeStates.ToList(),
                FillRate = FillRate
            };
        }

        public bool[,] To2DArray()
        {
            bool[,] state = new bool[Width,Height];
            var positions = GetPositions();

            foreach (var p in positions)
            {
                state[p.X, p.Y] = IsAlive(p.X, p.Y);
            }

            return state;
        }

        /// <summary>
        /// Gets all positions coordinates of the world.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Position> GetPositions()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    yield return new Position
                    {
                        X = i,
                        Y = j
                    };
                }
            }
        }

        /// <summary>
        /// Gets the adjacent positions to given coordinates in the world.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns></returns>
        public IEnumerable<Position> GetAdjacentPositions(int x, int y)
        {
            var modifiers = new[]
            {
                new {x = -1, y = -1},
                new {x = -1, y = 0},
                new {x = -1, y = 1},
                new {x = 1, y = -1},
                new {x = 1, y = 0},
                new {x = 1, y = 1},
                new {x = 0, y = -1},
                new {x = 0, y = 1}
            };

            return modifiers.Select(modifier => new Position
            {
                X = x + modifier.x,
                Y = y + modifier.y,
            });
        }
    }
}