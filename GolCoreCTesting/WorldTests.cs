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
            Assert.AreEqual(world.Width, 10);
            Assert.AreEqual(world.Height, 20);

            world = new World(15, 31);
            Assert.AreEqual(world.Width, 15);
            Assert.AreEqual(world.Height, 31);
        }
    }
}
