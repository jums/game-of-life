using System;
using System.Windows.Threading;

namespace Jums.GameOfLife.WindowsClient
{
    /// <summary>
    /// Automatic player of the game.
    /// </summary>
    internal class Player
    {
        private readonly object evolveLock = new object();
        private readonly Action draw;
        private readonly Action evolve;
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public Player(Action draw, Action evolve)
        {
            this.draw = draw;
            this.evolve = evolve;
        }

        public void Play(TimeSpan interval)
        {
            timer.Interval = interval;
            timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    SynchronizedEvolve();
                    draw();
                    timer.Start();
                };
            timer.Start();
        }

        private void SynchronizedEvolve()
        {
            lock (evolveLock)
            {
                evolve();
            }
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}