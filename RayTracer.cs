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
            scene.AddPrimitive(new Plane(new Vector3(0, -1, 0), -1));
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
            Vector3 screenPoint;
            Vector3 screenHorz = camera.TopRight - camera.TopLeft; //Horizontal vector of the screen
            Vector3 screenVert = camera.BottomLeft - camera.TopLeft; //Vertical vector of the screen
            float horzStep = 1f / screen.width;
            float vertStep = 1f / screen.height;
            float u = 0, v = 0;
            for (int y = 0; y < screen.height; ++y, u = 0, v+= vertStep)
                for(int x=0; x<screen.width; ++x, u += horzStep)
                {
                    screenPoint = camera.TopLeft + u * screenHorz + v * screenVert; //Top left + u * horz + v * vert => screen point
                    Vector3 dir = screenPoint - camera.Position;  //A vector from the camera to that screen point
                    ray = new Ray(dir, camera.Position);  //Create a primary ray from there (dir is normalized in the constructer)

                    //foreach (Primitive p in scene.Primitives)
                    //   p.Intersect(ray);  //Calculate the intersection with all the primitives

                    byte i = (byte)(1024 / (ray.Intsect.Distance * ray.Intsect.Distance));
                    screen.pixels[x + screen.width * y] = CreateColor(ray.GetColor(scene)); // i << 16 ^ i << 8 ^ i;
                    //Draw some rays on the debug screen
                    if (y == (debugScreen.height/2) && x % 30 == 0)
                    {
                        debugScreen.Line(
                            TX(camera.Position.X, debugScreen),
                            TY(camera.Position.Z, debugScreen),
                            TX(camera.Position.X + ray.Intsect.Distance * ray.Direction.X, debugScreen),
                            TY(camera.Position.Z + ray.Intsect.Distance * ray.Direction.Z, debugScreen),
                            0xff0000);
                    }
                }
        }
    }
}
