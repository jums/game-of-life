using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Jums.GameOfLife.CoreC.Tests
{
    [TestFixture]
    class GrimReaperTests
    {
        GrimReaper reaper;
        World world;

        [TestFixtureSetUp]
        public void SetUp()
        {
            this.reaper = new GrimReaper();
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
            bool value = reaper.IsAlive(world, 1, 1);
        }

        [Test]
        public void IsAliveShouldBeFalseWhenOneAlive()
        {
            string data = @" --x
                             -x-
                             --- ";

            world.Import(ConvertToWorldData(data, 10, 10));
            bool value = reaper.IsAlive(world, 1, 1);
        }

        private static IEnumerable<bool> ConvertToWorldData(string data, int width, int height)
        {
            Regex replacer = new Regex(@"[^\n-x]");
            string trimmed = replacer.Replace(data, "");
            string[] rows = trimmed.Split('\n');
            var expandedRows = rows.Select(r => FillMissing(r, width, '-'));
            int missingRows = height - rows.Count();
            string columnsFilled = string.Join("", expandedRows);
            string final = FillMissing(columnsFilled, width * height, '-');
            return final.Select(t => t == 'x').ToArray();
        }

        private static string FillMissing(string original, int length, char character)
        {
            int missingChars = length - original.Length;
            if (missingChars == 0) return original;
            string addition = string.Join("", Enumerable.Repeat(character, missingChars));
            return original + addition;
        }
    }
}
