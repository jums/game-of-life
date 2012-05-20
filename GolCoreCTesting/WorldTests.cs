using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Jums.GameOfLife.CoreC.Tests
{
    [TestFixture]
    public class WorldTests
    {
        [Test]
        public void CreateWorld()
        {
            World world = new World(10, 20);
            Assert.AreEqual(10, world.Width);
            Assert.AreEqual(20, world.Height);

            world = new World(15, 31);
            Assert.AreEqual(15, world.Width);
            Assert.AreEqual(31, world.Height);
        }

        [Test]
        [ExpectedException("System.ArgumentOutOfRangeException")]
        public void CreateIrrationalWorldX()
        {
            World world = new World(World.MinimumSize - 1, 10);
        }

        [Test]
        [ExpectedException("System.ArgumentOutOfRangeException")]
        public void CreateIrrationalWorldY()
        {
            World world = new World(10, -1);
        }

        [Test]
        [ExpectedException("System.IndexOutOfRangeException")]
        public void CoordinateOverLimitX()
        {
            World world = new World(10, 10);
            bool alive = world.IsAlive(9, 10);
        }

        [Test]
        [ExpectedException("System.IndexOutOfRangeException")]
        public void CoordinateBelowLimitX()
        {
            World world = new World(10, 10);
            bool alive = world.IsAlive(-1, 0);
        }

        [Test]
        [ExpectedException("System.IndexOutOfRangeException")]
        public void CoordinateOverLimitY()
        {
            World world = new World(10, 10);
            bool alive = world.IsAlive(0, 10);
        }

        [Test]
        [ExpectedException("System.IndexOutOfRangeException")]
        public void CoordinateBelowLimitY()
        {
            World world = new World(10, 10);
            bool alive = world.IsAlive(5, -1);
        }

        [Test]
        public void CalculateCoordinateIndex()
        {
            World world = new World(10, 10);
            Assert.AreEqual(0, world.GetPositionIndex(0, 0));
            Assert.AreEqual(5, world.GetPositionIndex(5, 0));
            Assert.AreEqual(9, world.GetPositionIndex(9, 0));
            Assert.AreEqual(10, world.GetPositionIndex(0, 1));
            Assert.AreEqual(19, world.GetPositionIndex(9, 1));
            Assert.AreEqual(99, world.GetPositionIndex(9, 9));
            
            world = new World(14, 15);
            Assert.AreEqual(0, world.GetPositionIndex(0, 0));
            Assert.AreEqual(3, world.GetPositionIndex(3, 0));
            Assert.AreEqual(17, world.GetPositionIndex(3, 1));
            Assert.AreEqual(30, world.GetPositionIndex(2, 2));
            Assert.AreEqual(209, world.GetPositionIndex(13, 14));
        }

        [Test]
        public void InitialWorldIsDead()
        {
            World world = new World(10, 10);

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Assert.AreEqual(false, world.IsAlive(x, y));
                }
            }
        }

        [Test]
        public void SetFillRate()
        {
            World world = new World(20, 20);
            world.FillRate = 10;
            Assert.AreEqual(10, world.FillRate);
            world.FillRate = 90;
            Assert.AreEqual(90, world.FillRate);
            world.FillRate = 101;
            Assert.AreEqual(100, world.FillRate);
            world.FillRate = -5;
            Assert.AreEqual(0, world.FillRate);
        }

        [Test]
        public void RandomizeWorldAintTotallyDead()
        {
            World world = new World(20, 20);

            world.CreateLife();
            Assert.Greater(CountLife(world), 0);
        }

        [Test]
        public void RandomizeWorldFillsLifeWithFillRate()
        {
            World world = new World(20, 20);

            world.FillRate = 50;
            world.CreateLife();
            Assert.AreEqual(200, CountLife(world));

            world.FillRate = 66;
            world.CreateLife();
            Assert.AreEqual(264, CountLife(world));
        }

        [Test]
        public void RandomizeWorldIsRandom() // Ignoring the chance it may create the same world twice.
        {
            World world1 = new World(20, 20);
            world1.CreateLife();
            var life1 = GetLifeCoordinates(world1);

            World world2 = new World(20, 20);
            world2.CreateLife();
            var life2 = GetLifeCoordinates(world2);

            Assert.AreNotEqual(life1, life2);
        }

        private List<Point> GetLifeCoordinates(World world)
        {
            var coordinates = GetAllCoordinates(world);
            return coordinates.Where(c => world.IsAlive(c.X, c.Y)).ToList();
        }

        private int CountLife(World world)
        {
            var coordinates = GetAllCoordinates(world);
            return coordinates.Count(c => world.IsAlive(c.X, c.Y));
        }

        private static IEnumerable<Point> GetAllCoordinates(World world)
        {
            var coordinates =
                from x in Enumerable.Range(0, world.Width)
                from y in Enumerable.Range(0, world.Height)
                select new Point { X = x, Y = y };
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
