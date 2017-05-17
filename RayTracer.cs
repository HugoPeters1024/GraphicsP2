using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using static template.FunctionWrapper;

namespace template
{
    class RayTracer
    {
        Scene scene;
        Camera camera;

        public RayTracer()
        {
            camera = new Camera(new Vector3(0, 0, -3), new Vector3(0, 0, 1f));
            scene = new Scene();
            scene.AddPrimitive(new Sphere(new Vector3(0, 0, 0), 1f, new Vector3(1f)));
            scene.AddPrimitive(new Sphere(new Vector3(-1, 0, 0), 1f, new Vector3(0, 0, 1f)));
        }

        public void DrawRayTracer(Surface viewScreen, Surface debugScreen)
        {
            viewScreen.Print("RayTracer", 0, 0, 0xffffff);
            DrawDebug(debugScreen);
            Draw(viewScreen, debugScreen);
        }

        void DrawDebug(Surface debugScreen)
        {
            debugScreen.Print("Debug", 0, 0, 0x000000);
            scene.DrawDebug(debugScreen);
            camera.DrawDebug(debugScreen);
        }

        void Draw(Surface screen, Surface debugScreen)
        {
            Ray ray;
            for(int y=0; y<screen.height; ++y)
                for(int x=0; x<screen.width; ++x)
                {
                    float u = (float)x / screen.width;
                    float v = (float)y / screen.height;
                    Vector3 screenPoint = camera.TopLeft + u * (camera.TopRight - camera.TopLeft) + v * (camera.BottomLeft - camera.TopLeft);
                    Vector3 dir = screenPoint - camera.Position;
                    ray = new Ray(dir, camera.Position);

                    foreach (Primitive p in scene.Primitives)
                        p.Intersect(ray);

                    if (y == (debugScreen.height/2) && x % 40 == 0)
                    {
                        debugScreen.Line(
                            TX(camera.Position.X, debugScreen),
                            TY(camera.Position.Z, debugScreen),
                            TX(camera.Position.X + ray.Dist * ray.Direction.X, debugScreen),
                            TY(camera.Position.Z + ray.Dist * ray.Direction.Z, debugScreen),
                            0xff0000);
                    }
                    byte i = (byte)(1024 / (ray.Dist * ray.Dist));
                    screen.pixels[x + screen.width * y] = i << 16 ^ i << 8 ^ i;
                }
        }
    }
}
