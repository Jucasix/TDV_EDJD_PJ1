using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatmanGame
{
    internal class SpikeObstacle
    {
        public float x = 800;
        public readonly float y = 400;

        public SpikeObstacle(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public SpikeObstacle() { }
    }
}
