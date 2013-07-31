using System;
using System.Timers;
using System.Windows.Threading;

namespace GameOfLife
{
    class Player
    {
        private readonly object evolveLock = new object();
        private readonly Timer timer = new Timer();
        private readonly Action draw;
        private readonly Action evolve;
        private readonly Dispatcher dispatcher;

        public Player(Dispatcher dispatcher, Action draw, Action evolve)
        {
            this.dispatcher = dispatcher;
            this.draw = draw;
            this.evolve = evolve;
        }

        public void Play(int interval)
        {
            timer.Interval = interval;
            timer.Elapsed += (sender, args) =>
                {
                    timer.Stop();
                    InvokeEvolve();
                    InvokeDraw();
                    timer.Start();
                };
            timer.Start();
        }

        private void InvokeDraw()
        {
            dispatcher.Invoke(draw);
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
