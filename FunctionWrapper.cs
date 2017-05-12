using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template
{
    static class FunctionWrapper
    {
        public static int TX(float x, int sWidth)
        {
            return (int)(x + Game.SCENE_WIDTH) * sWidth;
        }

        public static int TY(float y, int sHeight)
        {
            return (int)(y + Game.SCENE_HEIGHT) * sHeight;
        }
    }
}
