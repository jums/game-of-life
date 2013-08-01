using System;
using System.Collections.Generic;
using System.Linq;

namespace Jums.GameOfLife.CoreCSharp
{
    /// <summary>
    /// Creates _seemingly_ random life on a world.
    /// </summary>
    internal class GreatMaker
    {
        private double fillRate = 10;

        public GreatMaker()
        {
        }

        /// <param name="fillRate">The fill rate of life as percentage.</param>
        public GreatMaker(double fillRate)
        {
            FillRate = fillRate;
        }

        /// <summary>
        /// The percentage of positions that should contain life.
        /// </summary>
        public double FillRate
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
        /// Fills the world with random life where amount of life versus death is
        /// precisely determined by the <c>FillRate</c>.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="seed">The random seed.</param>
        /// <returns>The random seed used.</returns>
        public int CreateLife(World world, int? seed = null)
        {
            IList<bool> lifeStates = world.State.ToList();
            int positionCount = lifeStates.Count;
            int lifeAmount = (int) Math.Round(FillRate/100f*positionCount);
            List<bool> lifePool = CreateLifeAndDeathPool(positionCount, lifeAmount);

            int theSeed = GetThisSeedOrDefault(seed);
            Random random = new Random(theSeed);

            for (int i = 0; i < positionCount; i++) // loop through actual positions
            {
                int lifeIndex = random.Next()%lifePool.Count;
                lifeStates[i] = lifePool[lifeIndex];
                lifePool.RemoveAt(lifeIndex);
            }

            world.Import(lifeStates);

            return theSeed;
        }

        private int GetThisSeedOrDefault(int? seed)
        {
            return seed ?? (int) (DateTime.Now.Ticks%Int32.MaxValue + GetHashCode());
        }

        private static List<bool> CreateLifeAndDeathPool(int positionCount, int lifeAmount)
        {
            List<bool> life = new List<bool>(positionCount);
            life.AddRange(Enumerable.Repeat(true, lifeAmount));
            life.AddRange(Enumerable.Repeat(false, positionCount - lifeAmount));
            return life;
        }
    }
}