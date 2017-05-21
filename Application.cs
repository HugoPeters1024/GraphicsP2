using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template
{
    class Application
    {
        RayTracer rayTracer;

        public Application()
        {
            
        }

        public void Draw(Surface viewScreen, Surface debugScreen)
        {
            rayTracer.Draw(viewScreen, debugScreen);
        }
    }
}
