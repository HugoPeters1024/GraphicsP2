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
        static Scene scene;
        static Camera camera;
        static Random random;

        public RayTracer()
        {
            camera = new Camera(new Vector3(0, 0, -3), new Vector3(0, 0, 1f));
            scene = new Scene();
            random = new Random();


            // scene.AddLight(new Light(new Vector3(1f, -.9f, -1.2f)) { Intensity = Vector3.One });
            // scene.AddLight(new Light(new Vector3(0, -0.9f, -1.2f)) { Intensity = Vector3.One });
            // scene.AddLight(new Light(new Vector3(0, 1, -2.2f)) { Intensity = new Vector3(0, 0, 1) * 1 });
            scene.AddLight(new Light(new Vector3(0, 2f, 0), new Vector3(1, 1, 0.4f) * 1));
            scene.AddLight(new Light(new Vector3(-2, -0.5f, 0), 20) { Intensity = new Vector3(0, 1, 0) });
            scene.AddLight(new Light(new Vector3(0, 1f, -2f), new Vector3(1, 0.5f, 1) * 10));
            scene.AddLight(new Light(new Vector3(-2, 1f, -2f), new Vector3(1, 0.5f, 1) * 10));
            scene.AddLight(new Light(new Vector3(2, 1f, -2f), new Vector3(1, 0.5f, 1) * 10));
            scene.AddLight(new Light(new Vector3(0, 2, 1), new Vector3(0, 0, 1)*100));

            scene.AddPrimitive(new Sphere(new Vector3(1.1f, 0, 0), 1f, new Vector3(1f)) { PrimitiveName = "Reflective Sphere", Reflectivity = 1f });
            scene.AddPrimitive(new Sphere(new Vector3(-1.1f, 0, 0), 1f, new Vector3(1f, 0.5f, 0.5f)) { PrimitiveName = "White Sphere", Reflectivity = 0f });
            //scene.AddPrimitive(new Sphere(new Vector3(-1, 0, 0), 1f, new Vector3(0, 0, 1f)) { PrimitiveName = "Blue Sphere"});
            scene.AddPrimitive(new Floor(new Vector3(0, 1, 0), -1f) { PrimitiveName = "Floor", Reflectivity = 0.5f});
            scene.AddPrimitive(new Plane(new Vector3(0, -1, 0), -5f) { PrimitiveName = "Roof" , Color = new Vector3(0, 0, 1)});
            scene.AddPrimitive(new Plane(new Vector3(0, 0, -1), -5f) { Color = new Vector3(1, 0, 0) });
            scene.AddPrimitive(new Plane(new Vector3(0, 0, 1), -5) { Color = new Vector3(0, 1, 1) });
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
            debugScreen.Line(0, 0, 0, debugScreen.height, 0xffff00);
            scene.DrawDebug(debugScreen);
            camera.DrawDebug(debugScreen);
        }

        void Draw(Surface screen, Surface debugScreen)
        {
            camera.Update();
            Ray ray;
            Vector3 screenPoint;
            Vector3 screenHorz = camera.TopRight - camera.TopLeft; //Horizontal vector of the screen
            Vector3 screenVert = camera.BottomLeft - camera.TopLeft; //Vertical vector of the screen
            float horzStep = 1f / screen.width;
            float vertStep = 1f / screen.height;
            float u = 0, v = 0;
            int offset = 0;
            for (int y = 0; y < screen.height; ++y, u = 0, v += vertStep, offset += screen.width)
                for (int x = 0; x < screen.width; ++x, u += horzStep)
                {
                    screenPoint = camera.TopLeft + u * screenHorz + v * screenVert; //Top left + u * horz + v * vert => screen point
                    Vector3 dir = screenPoint - camera.Position;  //A vector from the camera to that screen point
                    ray = new Ray(dir.Normalized(), camera.Position);  //Create a primary ray from there

                    //foreach (Primitive p in scene.Primitives)
                    //   p.Intersect(ray);  //Calculate the intersection with all the primitives

                    //byte i = (byte)(1024 / (ray.Intsect.Distance * ray.Intsect.Distance));
                    if (!camera.IsMoving)
                        screen.pixels[x + offset] = CreateColor(Clamp(ray.GetColor(scene)));
                    else
                    {
                        if (random.Next(15) == 0)
                            screen.pixels[x + offset] = CreateColor(Clamp(ray.GetStaticColor(scene) * Clamp(Vector3.Dot(ray.Direction, -ray.Intsect.Normal))));
                    }

                    //Draw some rays on the debug screen
                    if (y == (debugScreen.height >> 1) && x % 32 == 0)
                    {
                        /*
                            debugScreen.Line(
                            TX(camera.Position.X, debugScreen),
                            TY(camera.Position.Z, debugScreen),
                            TX(camera.Position.X + ray.Intsect.Distance * ray.Direction.X, debugScreen),
                            TY(camera.Position.Z + ray.Intsect.Distance * ray.Direction.Z, debugScreen),
                            0xff0000);

                        debugScreen.Line(
                            TX(camera.Position.X + ray.Intsect.Distance * ray.Direction.X, debugScreen),
                            TY(camera.Position.Z + ray.Intsect.Distance * ray.Direction.Z, debugScreen),
                            TX(camera.Position.X + ray.Intsect.Distance * ray.Direction.X + ray.Intsect.Normal.X, debugScreen),
                            TY(camera.Position.Z + ray.Intsect.Distance * ray.Direction.Z + ray.Intsect.Normal.Z, debugScreen),
                            0x00ff00); */

                        ray.DrawDebug(debugScreen, 0xff0000);
                    }
                }
        }

        #region Properties
        public static Scene Scene
        {
            get { return scene; }
        }

        public static Camera Camera
        {
            get { return camera; }
        }
        #endregion
    }
}
