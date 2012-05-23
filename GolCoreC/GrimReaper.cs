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

            IEnumerable<Position> positions = GetAdjacentPositions(x, y);
            bool currentlyAlive = world.IsAlive(x, y);

            return Law1(currentlyAlive, positions, world);
        }

        private IEnumerable<Position> GetAdjacentPositions(int x, int y)
        {
            var modifiers = new[]{
                new { x = -1, y = -1 },
                new { x = -1, y = 0 },
                new { x = -1, y = 1 },
                new { x = 1, y = -1 },
                new { x = 1, y = 0 },
                new { x = 1, y = 1 },
                new { x = 0, y = -1 },
                new { x = 0, y = 1 }
            };

            foreach (var modifier in modifiers)
            {
                yield return new Position { 
                    X = x + modifier.x,
                    Y = y + modifier.y,
                };

            }
        }

        /// <summary>
        /// 1. Any live cell with fewer than two live neighbours dies, as if caused by under-population.
        /// </summary>
        /// <param name="currentlyAlive">Is it currently alive.</param>
        /// <param name="adjacentPosition">The adjacent positions.</param>
        /// <param name="world">The world.</param>
        /// <returns>Should it be alive after evolution.</returns>
        private bool Law1(bool currentlyAlive, IEnumerable<Position> adjacentPosition, World world)
        {
            if (!currentlyAlive) return false;
            return adjacentPosition.Count(p => !world.IsAlive(p.X, p.Y)) < 2;
        }

        private struct Position
        {
            public int X;
            public int Y;
        }

    }
}
