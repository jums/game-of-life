using System;
using System.Linq;
using NUnit.Framework;

namespace Jums.GameOfLife.CoreCSharp.Tests
{
    [TestFixture]
    public class WorldTests
    {
        private World simpleWorld;
        private GreatMaker greatMaker;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            simpleWorld = new World(10, 10);
            greatMaker = new GreatMaker(10);
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
        public void CoordinatesOutsideNonWrappedWorldAreDead()
        {
            World world = new World(10, 10);
            Assert.False(world.IsAlive(9, 10));
            Assert.False(world.IsAlive(-1, 0));
            Assert.False(world.IsAlive(0, 10));
            Assert.False(world.IsAlive(5, -1));
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
        [ExpectedException(typeof (ArgumentNullException))]
        public void ImportShouldThrowFromNullData()
        {
            simpleWorld.Import(null);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void ImportShouldThrowFromDataWithTooSmallLength()
        {
            simpleWorld.Import(Enumerable.Repeat(false, 10*10 - 1));
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void ImportShouldThrowFromDataWithTooLargeLength()
        {
            simpleWorld.Import(Enumerable.Repeat(false, 10*10 + 1));
        }

        [Test]
        public void ImportShouldAcceptAllDead()
        {
            greatMaker.CreateLife(simpleWorld);
            simpleWorld.Import(Enumerable.Repeat(false, 10*10));
            Assert.True(simpleWorld.State.All(p => p == false));
        }

        [Test]
        public void ImportShouldAcceptAllAlive()
        {
            greatMaker.CreateLife(simpleWorld);
            simpleWorld.Import(Enumerable.Repeat(true, 10*10));
            Assert.True(simpleWorld.State.All(p => p));
        }

        [Test]
        public void ImportShouldAcceptRandomWorld()
        {
            greatMaker.CreateLife(simpleWorld);
            Random random = new Random();
            var randomData = Enumerable.Repeat(true, 10*10).Select(x => random.Next()%2 == 0).ToArray();
            simpleWorld.Import(randomData);
            CollectionAssert.AreEqual(randomData, simpleWorld.State);
        }

        [Test]
        public void CopyShouldHaveSameLifeStatesAndDimensions()
        {
            var world = new World(12, 14, true);
            greatMaker.CreateLife(world);
            World another = world.Copy();
            Assert.AreEqual(world.Width, another.Width);
            Assert.AreEqual(world.Height, another.Height);
            Assert.AreEqual(world.Wrapped, another.Wrapped);
            CollectionAssert.AreEqual(world.State, another.State);
        }

        [Test]
        public void CopyShouldNotBeSameObject()
        {
            World another = simpleWorld.Copy();
            Assert.AreNotSame(simpleWorld, another);
        }

        [Test]
        public void EmptyCopyShouldHaveSameSettingsNoLife()
        {
            var world = new World(12, 14, true);
            greatMaker.CreateLife(world);
            World another = world.CopyEmpty();
            Assert.AreEqual(world.Width, another.Width);
            Assert.AreEqual(world.Height, another.Height);
            Assert.AreEqual(world.Wrapped, another.Wrapped);
            CollectionAssert.AreNotEqual(world.State, another.State);
            Assert.True(another.State.All(s => !s));
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
    }
}