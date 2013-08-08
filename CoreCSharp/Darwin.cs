using System;
using System.Collections.Generic;
using System.Linq;

namespace Jums.GameOfLife.CoreCSharp
{
    /// <summary>
    /// Master of evolution. Handles birth, life and death through the ages.
    /// </summary>
    internal class Darwin
    {
        public World Evolve(World world)
        {
            var evolution = world.CopyEmpty();
            var positions = world.GetPositions().ToList();
            var newStates = positions.AsParallel().Select(p => IsAlive(world, p.X, p.Y)).ToList();
            evolution.Import(newStates);
            return evolution;
        }

        public bool IsAlive(World world, int x, int y)
        {
            if (world == null) throw new ArgumentNullException("world");

            IEnumerable<Position> positions = world.GetAdjacentPositions(x, y);
            bool currentlyAlive = world.IsAlive(x, y);

            return ApplyLaws(currentlyAlive, positions, world);
        }

        /// <summary>
        /// Applies the laws:
        /// 1. Any live cell with fewer than two live neighbours dies, as if caused by under-population.
        /// 2. Any live cell with two or three live neighbours lives on to the next generation
        /// 3. Any live cell with more than three live neighbours dies, as if by overcrowding.
        /// 4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
        /// </summary>
        /// <param name="currentlyAlive">if set to <c>true</c> [currently alive].</param>
        /// <param name="adjacentPositions">The adjacent positions.</param>
        /// <param name="world">The world.</param>
        /// <returns></returns>
        private bool ApplyLaws(bool currentlyAlive, IEnumerable<Position> adjacentPositions, World world)
        {
            int adjacentAlive = adjacentPositions.Count(p => world.IsAlive(p.X, p.Y));

            if (currentlyAlive)
                return adjacentAlive == 2 || adjacentAlive == 3;
            else
                return adjacentAlive == 3;
        }
    }
}