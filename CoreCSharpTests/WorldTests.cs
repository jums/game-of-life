using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Jums.GameOfLife.CoreCSharp.Tests
{
    [TestFixture]
    public class WorldTests
    {
        World simpleWorld;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            simpleWorld = new World(10, 10);
        }

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
            new World(World.MinimumSize - 1, 10);
        }

        [Test]
        [ExpectedException("System.ArgumentOutOfRangeException")]
        public void CreateIrrationalWorldY()
        {
            new World(10, -1);
        }

        [Test]
        [ExpectedException("System.IndexOutOfRangeException")]
        public void CoordinateOverLimitX()
        {
            World world = new World(10, 10);
            world.IsAlive(9, 10);
        }

        [Test]
        [ExpectedException("System.IndexOutOfRangeException")]
        public void CoordinateBelowLimitX()
        {
            World world = new World(10, 10);
            world.IsAlive(-1, 0);
        }

        [Test]
        [ExpectedException("System.IndexOutOfRangeException")]
        public void CoordinateOverLimitY()
        {
            World world = new World(10, 10);
            world.IsAlive(0, 10);
        }

        [Test]
        [ExpectedException("System.IndexOutOfRangeException")]
        public void CoordinateBelowLimitY()
        {
            World world = new World(10, 10);
            world.IsAlive(5, -1);
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

            CollectionAssert.AreNotEquivalent(life1, life2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImportShouldThrowFromNullData()
        {
            simpleWorld.Import(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ImportShouldThrowFromDataWithTooSmallLength()
        {
            simpleWorld.Import(Enumerable.Repeat(false, 10 * 10 - 1));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ImportShouldThrowFromDataWithTooLargeLength()
        {
            simpleWorld.Import(Enumerable.Repeat(false, 10 * 10 + 1));
        }

        [Test]
        public void ImportShouldAcceptAllDead()
        {
            simpleWorld.CreateLife();
            simpleWorld.Import(Enumerable.Repeat(false, 10 * 10));
            Assert.True(simpleWorld.Positions.All(p => p == false));
        }

        [Test]
        public void ImportShouldAcceptAllAlive()
        {
            simpleWorld.CreateLife();
            simpleWorld.Import(Enumerable.Repeat(true, 10 * 10));
            Assert.True(simpleWorld.Positions.All(p => p == true));
        }

        [Test]
        public void ImportShouldAcceptRandomWorld()
        {
            simpleWorld.CreateLife();
            Random random = new Random();
            var randomData = Enumerable.Repeat(true, 10 * 10).Select(x => random.Next() % 2 == 0).ToArray();
            simpleWorld.Import(randomData);
            CollectionAssert.AreEqual(randomData, simpleWorld.Positions);
        }

        [Test]
        public void CopyShouldHaveSameLifeStatesAndDimensions()
        {
            var world = new World(12, 14, true);
            world.FillRate = 26;
            world.CreateLife();
            World another = world.Copy();
            Assert.AreEqual(world.Width, another.Width);
            Assert.AreEqual(world.Height, another.Height);
            Assert.AreEqual(world.FillRate, another.FillRate);
            Assert.AreEqual(world.Wrapped, another.Wrapped);
            CollectionAssert.AreEqual(world.Positions, another.Positions);
        }

        [Test]
        public void CopyShouldNotBeSameObject()
        {
            World another = simpleWorld.Copy();
            Assert.AreNotSame(simpleWorld, another);
        }

        [Test]
        public void RegularAdjacentPositions()
        {
            var positions = simpleWorld.GetAdjacentPositions(5, 5);

            var expected = new[]
            {
                new {x = 4, y = 4},
                new {x = 5, y = 4},
                new {x = 6, y = 4},
                new {x = 4, y = 6},
                new {x = 5, y = 6},
                new {x = 6, y = 6},
                new {x = 4, y = 5},
                new {x = 6, y = 5}
            };

            Assert.True(expected.All(p => positions.Any(pp => pp.X == p.x && pp.Y == p.y)));
        }

        [Test]
        public void NonWrappedAdjacentPositionsAtCorner()
        {
            var positions = simpleWorld.GetAdjacentPositions(0, 0);

            var expected = new[]
            {
                new {x = -1, y = -1},
                new {x = 0, y = -1},
                new {x = 1, y = -1},
                new {x = -1, y = 1},
                new {x = 0, y = 1},
                new {x = 1, y = 1},
                new {x = -1, y = 0},
                new {x = 1, y = 0}
            };

            Assert.True(expected.All(p => positions.Any(pp => pp.X == p.x && pp.Y == p.y)));
        }

        [Test]
        public void WrappedAdjacentPositionsAtCorner()
        {
            var world = new World(20, 10, true);
            var positions = world.GetAdjacentPositions(0, 0);

            var expected = new[]
            {
                new {x = 19, y = 9},
                new {x = 0, y = 9},
                new {x = 1, y = 9},
                new {x = 19, y = 1},
                new {x = 0, y = 1},
                new {x = 1, y = 1},
                new {x = 19, y = 0},
                new {x = 1, y = 0}
            };

            string poss = string.Join(" | ", positions.Select(p => p.ToString()));

            Assert.True(expected.All(p => positions.Any(pp => pp.X == p.x && pp.Y == p.y)), poss);
        }

        [Test]
        public void WrappedAdjacentPositionsAtAnotherCorner()
        {
            var world = new World(20, 10, true);
            var positions = world.GetAdjacentPositions(19, 0);

            var expected = new[]
            {
                new {x = 18, y = 9},
                new {x = 19, y = 9},
                new {x = 0, y = 9},
                new {x = 18, y = 1},
                new {x = 19, y = 1},
                new {x = 0, y = 1},
                new {x = 18, y = 0},
                new {x = 0, y = 0}
            };

            Assert.True(expected.All(p => positions.Any(pp => pp.X == p.x && pp.Y == p.y)));
        }

        private List<Point> GetLifeCoordinates(World world)
        {
            var coordinates = GetAllCoordinates(world);
            return coordinates.Where(c => world.IsAlive(c.X, c.Y)).ToList();
        }

        private int CountLife(World world)
        {
            return world.Positions.Count(p => p == true);
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
