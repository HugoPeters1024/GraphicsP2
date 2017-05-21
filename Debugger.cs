﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template
{
    static class Debugger
    {
        static Ray[] primaryRays;
        static Ray[] shadowRays;
        static Ray[] reflectedRays;
        static List<Ray> primaryRaysBuffer;
        static List<Ray> shadowRaysBuffer;
        static List<Ray> reflectedRaysBuffer;
        static Surface screen;
        static Scene scene;
        static Camera camera;

        public static void Init(Surface _screen, Scene _scene, Camera _camera)
        {
            primaryRays = new Ray[0];
            shadowRays = new Ray[0];
            reflectedRays = new Ray[0];
            primaryRaysBuffer = new List<Ray>();
            shadowRaysBuffer = new List<Ray>();
            reflectedRaysBuffer = new List<Ray>();
            screen = _screen;
            scene = _scene;
            camera = _camera;
        }

        public static void Draw()
        {
            foreach(Ray r in primaryRays)
                r.DrawDebug(screen, 0xff0000, false);
            foreach (Ray s in shadowRays)
                s.DrawDebug(screen, 0x0000ff, true);
            foreach (Ray r in reflectedRays)
                r.DrawDebug(screen, 0x00ff00, false);
        }

        public static void AddPrimaryRay(Ray r)
        {
               primaryRaysBuffer.Add(r);
        }

        public static void AddShadowRay(Ray s)
        {
            shadowRaysBuffer.Add(s);
        }

        public static void AddReflectedRay(Ray r)
        {
            reflectedRaysBuffer.Add(r);
        }

        public static void SwapBuffers()
        {
            primaryRays = new Ray[primaryRaysBuffer.Count];
            primaryRaysBuffer.CopyTo(primaryRays);
            shadowRays = new Ray[shadowRaysBuffer.Count];
            shadowRaysBuffer.CopyTo(shadowRays);
            reflectedRays = new Ray[reflectedRaysBuffer.Count];
            reflectedRaysBuffer.CopyTo(reflectedRays);
            primaryRaysBuffer.Clear();
            shadowRaysBuffer.Clear();
            reflectedRaysBuffer.Clear();
            screen.Clear(0);
            DrawDebug();
        }

        public static void DrawDebug()
        {
            screen.Line(0, 0, 0, screen.height, 0xffff00);
            scene.DrawDebug(screen);
            camera.DrawDebug(screen);
            Draw();
        }


    }
}
