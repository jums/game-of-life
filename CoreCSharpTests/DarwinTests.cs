using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Jums.GameOfLife.CoreCSharp.Tests
{
    [TestFixture]
    internal class DarwinTests
    {
        private Darwin darwin;
        private World commonWorld;

        [TestFixtureSetUp]
        public void SetUp()
        {
            darwin = new Darwin();
            commonWorld = new World(10, 10);
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void IsAliveShouldThrowWithNullWorld()
        {
            darwin.Evolve(null);
        }

        [Test]
        public void IsAliveShouldBeFalseWhenZeroAlive()
        {
            string data = @" ---
                             -x-
                             --- ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));
        }

        [Test]
        public void IsAliveShouldBeFalseWhenOneAlive()
        {
            string data1 = @" --x
                              -x-
                              --- ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data1));

            string data2 = @" ---
                              -x-
                              -x- ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data2));
        }

        [Test]
        public void IsAliveShouldBeTrueWhenTwoAlive()
        {
            string data1 = @" -xx
                              -x-
                              --- ";

            Assert.True(IsCenterAliveAfterEvolution(commonWorld, data1));

            string data2 = @" x--
                              -x-
                              -x- ";

            Assert.True(IsCenterAliveAfterEvolution(commonWorld, data2));
        }

        [Test]
        public void IsAliveShouldBeTrueWhenThreeAlive()
        {
            string data1 = @" xxx
                              -x-
                              --- ";

            Assert.True(IsCenterAliveAfterEvolution(commonWorld, data1));

            string data2 = @" x--
                              -x-
                              -xx ";

            Assert.True(IsCenterAliveAfterEvolution(commonWorld, data2));
        }

        [Test]
        public void IsAliveShouldBeFalseWhenFourOrMoreAlive()
        {
            string data = @" xxx
                             -x-
                             x-- ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));

            data = @" x--
                      -xx
                      -xx ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));

            data = @" x--
                      xxx
                      xx- ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));

            data = @" x-x
                      xxx
                      -xx ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));

            data = @" xxx
                      xxx
                      -xx ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));

            data = @" xxx
                      xxx
                      xxx ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));
        }

        [Test]
        public void IsAliveShouldBeTrueeWhenExactlyThreeAliveAndCurrentDead()
        {
            string data = @" x-x
                             ---
                             x-- ";

            Assert.True(IsCenterAliveAfterEvolution(commonWorld, data));

            data = @" xxx
                      ---
                      --- ";

            Assert.True(IsCenterAliveAfterEvolution(commonWorld, data));

            data = @" x--
                      --x
                      -x- ";

            Assert.True(IsCenterAliveAfterEvolution(commonWorld, data));
        }

        [Test]
        public void IsAliveShouldBeFalseWhenTwoOrOneAliveAndCurrentDead()
        {
            string data = @" x--
                             ---
                             x-- ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));

            data = @" ---
                      --x
                      --- ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));
        }

        [Test]
        public void IsAliveShouldBeFalseWhenFourOrMoreAliveAndCurrentDead()
        {
            string data = @" xxx
                             ---
                             x-- ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));

            data = @" x--
                      --x
                      -xx ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));

            data = @" x--
                      x-x
                      xx- ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));

            data = @" x-x
                      x-x
                      -xx ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));

            data = @" xxx
                      x-x
                      -xx ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));

            data = @" xxx
                      x-x
                      xxx ";

            Assert.False(IsCenterAliveAfterEvolution(commonWorld, data));
        }

        private bool IsCenterAliveAfterEvolution(World world, string data)
        {
            var worldState = ConvertToWorldData(data, world.Width, world.Height);
            world.Import(worldState);
            var evolution = darwin.Evolve(world);
            return evolution.IsAlive(1, 1);
        }

        private static IEnumerable<bool> ConvertToWorldData(string data, int width, int height)
        {
            Regex replacer = new Regex(@"[^\n-x]+");
            string trimmed = replacer.Replace(data, "");
            string[] rows = trimmed.Split('\n');
            var expandedRows = rows.Select(r => FillMissing(r, width, '-')).ToList();
            string columnsFilled = string.Join("", expandedRows);
            string final = FillMissing(columnsFilled, width*height, '-');
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