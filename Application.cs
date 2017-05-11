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
            rayTracer = new RayTracer();
        }

        public void Draw(Surface viewScreen, Surface debugScreen)
        {
            rayTracer.DrawRayTracer(viewScreen);
            rayTracer.DrawDebug(debugScreen);
        }
    }
}
