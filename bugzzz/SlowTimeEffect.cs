using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bugzzz
{
    // Causes a matrix like time slow effect
    // Triggered by random chance on killing an enemy
    class SlowTimeEffect
    {
        public bool isActive;
        public int counter;
        public int length;

        public SlowTimeEffect()
        {
            this.isActive = false;
            this.counter = 1;
            this.length = 250;
        }
    }
}
