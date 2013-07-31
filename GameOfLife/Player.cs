using System;
using System.Timers;
using System.Windows.Threading;

namespace GameOfLife
{
    class Player
    {
        private readonly object evolveLock = new object();
        private readonly Action draw;
        private readonly Action evolve;
        readonly DispatcherTimer timer = new DispatcherTimer();

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
                    InvokeEvolve();
                    InvokeDraw();
                };
            timer.Start();
        }

        private void InvokeDraw()
        {
            draw();
        }

        private void InvokeEvolve()
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
