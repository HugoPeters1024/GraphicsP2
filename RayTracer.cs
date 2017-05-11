using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template
{
    class RayTracer
    {
        Scene scene;
        Camera camera;

        public RayTracer()
        {
        }

        public void DrawRayTracer(Surface viewScreen)
        {
            viewScreen.Print("RayTracer", 0, 0, 0xffffff);
        }

        public void DrawDebug(Surface debugScreen)
        {
            debugScreen.Print("Debug", 0, 0, 0x000000);
        }
    }
}
