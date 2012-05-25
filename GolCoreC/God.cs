using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jums.GameOfLife.CoreC
{
    /// <summary>
    /// Manages a world in all points of time in every way. Grim reaper works for god.
    /// </summary>
    class God
    {
        public God(World initialWorld, GrimReaper grimReaper)
        {
            this.WorldNow = initialWorld;
            this.WorldTomorrow = initialWorld.Copy();
            this.GrimReaper = grimReaper;
        }

        private World WorldNow { get; set; }
        private World WorldTomorrow { get; set; }
        private GrimReaper GrimReaper { get; set; }
    }
}
