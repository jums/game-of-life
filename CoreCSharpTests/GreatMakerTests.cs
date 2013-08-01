using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Jums.GameOfLife.CoreCSharp.Tests
{
    [TestFixture]
    public class GreatMakerTests
    {
        [Test]
        public void SetFillRate()
        {
            var maker = new GreatMaker();
            Assert.GreaterOrEqual(maker.FillRate, 0);
            Assert.LessOrEqual(maker.FillRate, 100);
            maker.FillRate = 10;
            Assert.AreEqual(10, maker.FillRate);
            maker.FillRate = 90;
            Assert.AreEqual(90, maker.FillRate);
            maker.FillRate = 101;
            Assert.AreEqual(100, maker.FillRate);
            maker.FillRate = -5;
            Assert.AreEqual(0, maker.FillRate);
        }

        [Test]
        public void RandomizeWorldFillsLifeWithFillRate()
        {
            var world = new World(20, 20);
            var maker = new GreatMaker(50);
            maker.CreateLife(world);
            Assert.AreEqual(200, CountLife(world));

            maker.FillRate = 66;
            maker.CreateLife(world);
            Assert.AreEqual(264, CountLife(world));
        }

        [Test]
        public void RandomizeWorldIsRandom() // Ignoring the chance it may create the same world twice.
        {
            var world1 = new World(20, 20);
            var world2 = new World(20, 20);
            var maker = new GreatMaker(10);

            maker.CreateLife(world1, 1);
            maker.CreateLife(world2, 2);

            var life1 = GetLifeCoordinates(world1);
            var life2 = GetLifeCoordinates(world2);

            CollectionAssert.AreNotEquivalent(life1, life2);
        }

        private List<Point> GetLifeCoordinates(World world)
        {
            var coordinates = GetAllCoordinates(world);
            return coordinates.Where(c => world.IsAlive(c.X, c.Y)).ToList();
        }

        private int CountLife(World world)
        {
            return world.State.Count(p => p);
        }

        private static IEnumerable<Point> GetAllCoordinates(World world)
        {
            var coordinates =
                from x in Enumerable.Range(0, world.Width)
                from y in Enumerable.Range(0, world.Height)
                select new Point {X = x, Y = y};
            return coordinates;
        }

        private struct Point
        {
            public int X;
            public int Y;

            public override string ToString()
            {
                return string.Format("x:{0} y:{1}", X, Y);
            }
        }
    }
}