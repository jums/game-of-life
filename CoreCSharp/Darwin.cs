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
        private List<Func<bool, int, World, bool?>> Laws
        {
            get
            {
                return new List<Func<bool, int, World, bool?>>
                {
                    Law1, Law2, Law3, Law4
                };
            }
        }

        public World Evolve(World world)
        {
            var evolution = world.Copy();
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

            return ProcessRules(currentlyAlive, positions, world);
        }

        private bool ProcessRules(bool currentlyAlive, IEnumerable<Position> adjacentPositions, World world)
        {
            int adjacentAlive = adjacentPositions.Count(p => world.IsAlive(p.X, p.Y));
            bool? result = null;

            foreach (var law in Laws)
            {
                result = law(currentlyAlive, adjacentAlive, world);
                if (result != null) break;
            }

            return result ?? false;
        }

        /// <summary>
        /// 1. Any live cell with fewer than two live neighbours dies, as if caused by under-population.
        /// </summary>
        /// <param name="currentAlive">Is it currently alive.</param>
        /// <param name="adjacentAlive">Amount of adjacent life.</param>
        /// <param name="world">The world.</param>
        /// <returns>Should it be alive after evolution. Null means the rule does not apply.</returns>
        private bool? Law1(bool currentAlive, int adjacentAlive, World world)
        {
            if (!currentAlive) return null;
            if (adjacentAlive < 2) return false;
            return null;
        }

        /// <summary>
        /// 2. Any live cell with two or three live neighbours lives on to the next generation.
        /// </summary>
        /// <param name="currentAlive">Is it currently alive.</param>
        /// <param name="adjacentAlive">Amount of adjacent life.</param>
        /// <param name="world">The world.</param>
        /// <returns>Should it be alive after evolution. Null means the rule does not apply.</returns>
        private bool? Law2(bool currentAlive, int adjacentAlive, World world)
        {
            if (!currentAlive) return null;
            if (adjacentAlive == 2 || adjacentAlive == 3) return true;
            return null;
        }

        /// <summary>
        /// 3. Any live cell with more than three live neighbours dies, as if by overcrowding.
        /// </summary>
        /// <param name="currentAlive">Is it currently alive.</param>
        /// <param name="adjacentAlive">Amount of adjacent life.</param>
        /// <param name="world">The world.</param>
        /// <returns>Should it be alive after evolution. Null means the rule does not apply.</returns>
        private bool? Law3(bool currentAlive, int adjacentAlive, World world)
        {
            if (!currentAlive) return null;
            if (adjacentAlive > 3) return false;
            return null;
        }

        /// <summary>
        /// 4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
        /// </summary>
        /// <param name="currentAlive">Is it currently alive.</param>
        /// <param name="adjacentAlive">Amount of adjacent life.</param>
        /// <param name="world">The world.</param>
        /// <returns>Should it be alive after evolution. Null means the rule does not apply.</returns>
        private bool? Law4(bool currentAlive, int adjacentAlive, World world)
        {
            if (currentAlive) return null;
            if (adjacentAlive == 3) return true;
            return null;
        }
    }
}