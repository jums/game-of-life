using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Jums.GameOfLife.CoreCSharp.Tests
{
    [TestFixture]
    class GrimReaperTests
    {
        MotherNature reaper;
        World world;

        [TestFixtureSetUp]
        public void SetUp()
        {
            this.reaper = new MotherNature();
            this.world = new World(10, 10);
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void IsAliveShouldThrowWithNullWorld()
        {
            this.reaper.IsAlive(null, 1, 1);
        }

        [Test]
        public void IsAliveShouldBeFalseWhenZeroAlive()
        {
            string data = @" ---
                             -x-
                             --- ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));
        }

        [Test]
        public void IsAliveShouldBeFalseWhenOneAlive()
        {
            string data1 = @" --x
                              -x-
                              --- ";

            world.Import(ConvertToWorldData(data1, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));

            string data2 = @" ---
                              -x-
                              -x- ";

            world.Import(ConvertToWorldData(data2, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));
        }

        [Test]
        public void IsAliveShouldBeTrueWhenTwoAlive()
        {
            string data1 = @" -xx
                              -x-
                              --- ";

            world.Import(ConvertToWorldData(data1, 10, 10));
            Assert.True(reaper.IsAlive(world, 1, 1));

            string data2 = @" x--
                              -x-
                              -x- ";

            world.Import(ConvertToWorldData(data2, 10, 10));
            Assert.True(reaper.IsAlive(world, 1, 1));
        }

        [Test]
        public void IsAliveShouldBeTrueWhenThreeAlive()
        {
            string data1 = @" xxx
                              -x-
                              --- ";

            world.Import(ConvertToWorldData(data1, 10, 10));
            Assert.True(reaper.IsAlive(world, 1, 1));

            string data2 = @" x--
                              -x-
                              -xx ";

            world.Import(ConvertToWorldData(data2, 10, 10));
            Assert.True(reaper.IsAlive(world, 1, 1));
        }


        [Test]
        public void IsAliveShouldBeFalseWhenFourOrMoreAlive()
        {
            string data = @" xxx
                             -x-
                             x-- ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));

            data = @" x--
                      -xx
                      -xx ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));

            data = @" x--
                      xxx
                      xx- ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));

            data = @" x-x
                      xxx
                      -xx ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));

            data = @" xxx
                      xxx
                      -xx ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));

            data = @" xxx
                      xxx
                      xxx ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));
        }

        [Test]
        public void IsAliveShouldBeTrueeWhenExactlyThreeAliveAndCurrentDead()
        {
            string data = @" x-x
                             ---
                             x-- ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.True(reaper.IsAlive(world, 1, 1));

            data = @" xxx
                      ---
                      --- ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.True(reaper.IsAlive(world, 1, 1));

            data = @" x--
                      --x
                      -x- ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.True(reaper.IsAlive(world, 1, 1));
        }

        [Test]
        public void IsAliveShouldBeFalseWhenTwoOrOneAliveAndCurrentDead()
        {
            string data = @" x--
                             ---
                             x-- ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));

            data = @" ---
                      --x
                      --- ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));
        }

        [Test]
        public void IsAliveShouldBeFalseWhenFourOrMoreAliveAndCurrentDead()
        {
            string data = @" xxx
                             ---
                             x-- ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));

            data = @" x--
                      --x
                      -xx ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));

            data = @" x--
                      x-x
                      xx- ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));

            data = @" x-x
                      x-x
                      -xx ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));

            data = @" xxx
                      x-x
                      -xx ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));

            data = @" xxx
                      x-x
                      xxx ";

            world.Import(ConvertToWorldData(data, 10, 10));
            Assert.False(reaper.IsAlive(world, 1, 1));
        }

        private static IEnumerable<bool> ConvertToWorldData(string data, int width, int height)
        {
            Regex replacer = new Regex(@"[^\n-x]+");
            string trimmed = replacer.Replace(data, "");
            string[] rows = trimmed.Split('\n');
            var expandedRows = rows.Select(r => FillMissing(r, width, '-')).ToList();
            string columnsFilled = string.Join("", expandedRows);
            string final = FillMissing(columnsFilled, width * height, '-');
            return final.Select(t => t == 'x').ToArray();
        }

        private static string FillMissing(string original, int length, char character)
        {
            original = original.Trim();
            int missingChars = length - original.Length;
            if (missingChars == 0) return original;
            string addition = string.Join("", Enumerable.Repeat(character.ToString(), missingChars));
            return original + addition;
        }
    }
}
