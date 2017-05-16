﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template
{
    class RayTracer
    {
        Scene scene;
        Camera camera;

        public RayTracer()
        {
            camera = new Camera(new Vector3(0, -2, 0), new Vector3(0, 1f, 0));
            scene = new Scene();
            scene.AddPrimitive(new Sphere(new Vector3(0, 2f, 0), 1f, new Vector3(0)));
        }

        public void DrawRayTracer(Surface viewScreen)
        {
            viewScreen.Print("RayTracer", 0, 0, 0xffffff);
        }

        public void DrawDebug(Surface debugScreen)
        {
            debugScreen.Print("Debug", 0, 0, 0x000000);
            scene.DrawDebug(debugScreen);
            camera.DrawDebug(debugScreen);
        }
    }
}
