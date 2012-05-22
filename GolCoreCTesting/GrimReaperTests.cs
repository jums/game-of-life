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

        [TestFixtureSetUp]
        public void SetUp()
        {
            this.reaper = new GrimReaper();
        }

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void IsAliveShouldDiscardNullWorld()
        {
            this.reaper.IsAlive(null, 1, 1);
        }

        [Test]
        public void IsAliveShouldBeDeadWhenAllIsDead()
        {
            string data = @"
                ----------
                -x--------
                ----------
                ----------
                ----------
                ----------
                ----------
                ----------
                ----------
                ----------
";



        }

        public static IEnumerable<bool> ConvertToWorldData(string data, int width, int height)
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
