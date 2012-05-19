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

            var coordinates =
                from x in Enumerable.Range(0, 20)
                from y in Enumerable.Range(0, 20)
                select new { x, y };

            int lifeCount = coordinates.Count(c => world.IsAlive(c.x, c.y));
            Assert.Greater(lifeCount, 0);
        }
    }
}
