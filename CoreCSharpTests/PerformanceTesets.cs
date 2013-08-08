using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Jums.GameOfLife.CoreCSharp.Tests
{
    /// <summary>
    /// Subjective performance tests. On my old 4-core system these are acceptable.
    /// </summary>
    [TestFixture]
    internal class PerformanceTesets
    {
        [Test]
        public void LargeRandomWorldEvolution100()
        {
            World world = new World(250, 150);
            Darwin darwin = new Darwin();
            GreatMaker maker = new GreatMaker(15);
            maker.CreateLife(world);

            const int evolutions = 100;
            TimeSpan acceptedTime = TimeSpan.FromSeconds(1);

            TimeSpan actualTime = Time(() =>
                {
                    for (int i = 0; i < evolutions; i++)
                    {
                        world = darwin.Evolve(world);
                    }
                });

            Assert.LessOrEqual(actualTime, acceptedTime);
        }

        [Test]
        public void LargeRandomWorldEvolutionWithStateRetrieval100()
        {
            World world = new World(250, 150);
            Darwin darwin = new Darwin();
            GreatMaker maker = new GreatMaker(15);
            maker.CreateLife(world);

            const int evolutions = 100;
            TimeSpan acceptedTime = TimeSpan.FromSeconds(1);

            TimeSpan actualTime = Time(() =>
            {
                for (int i = 0; i < evolutions; i++)
                {
                    world = darwin.Evolve(world);
                    var z = world.To2DArray();

                }
            });

            Assert.LessOrEqual(actualTime, acceptedTime);
        }

        public static TimeSpan Time(Action action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}