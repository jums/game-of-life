using System;
using System.Collections.Generic;
using System.Linq;
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
        private IList<bool> lifeStates;

        public World(int width, int height, bool wrapped) : this(width, height)
        {
            Wrapped = wrapped;
        }

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
        public bool Wrapped { get; set; } // Yeah, a flat world can suddenly become round, like in real life.

        /// <summary>
        /// Enumeration of all positions life states in the world.
        /// </summary>
        public IEnumerable<bool> State
        {
            get { return new ReadOnlyCollectionBuilder<bool>(lifeStates).ToReadOnlyCollection(); }
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
        /// Makes a complete copy of this world.
        /// </summary>
        /// <returns></returns>
        public World Copy()
        {
            return new World(Width, Height, Wrapped)
            {
                lifeStates = lifeStates.ToList()
            };
        }

        /// <summary>
        /// Makes an empty copy of this world.
        /// </summary>
        /// <returns></returns>
        public World CopyEmpty()
        {
            return new World(Width, Height, Wrapped);
        }

        /// <summary>
        /// Expresses life states as a 2D bool array.
        /// </summary>
        /// <returns></returns>
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
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    yield return new Position(x, y);
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

            var positions = modifiers.Select(m => new Position(x + m.x, y + m.y));

            if (Wrapped)
            {
                positions = Wrap(positions);
            }

            return positions;
        }

        /// <summary>
        /// Sets life state at given cell coordinates.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="isAlive">if set to <c>true</c> the cell is alive.</param>
        public void SetLifeAt(int x, int y, bool isAlive)
        {
            int index = GetPositionIndex(x, y);
            lifeStates[index] = isAlive;
        }

        private IEnumerable<Position> Wrap(IEnumerable<Position> positions)
        {
            return positions.Select(Wrap);
        }

        private Position Wrap(Position position)
        {
            return new Position(
                x: WrapDimension(position.X, Width),
                y: WrapDimension(position.Y, Height)
                );
        }

        private int WrapDimension(int x, int max)
        {
            if (x < 0) return max + x;
            if (x >= max) return x - max;
            return x;
        }
    }
}