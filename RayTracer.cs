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

        static int MSAA;
        static float msaaValue;
        float axisoffsetX, axisoffsetY;
        public static int y = 0;
        float u = 0, v = 0;
        int offset = 0;

        public RayTracer()
        {
            camera = new Camera(new Vector3(0, 0, -3), new Vector3(0, 0, 1f));
            scene = new Scene();
            random = new Random();
            MSAA = 1;
            msaaValue = (float)Math.Sqrt(MSAA);
            axisoffsetX = 1f / (OpenTKApp.VIEW_WIDTH * msaaValue);
            axisoffsetY = 1f / (OpenTKApp.VIEW_HEIGHT * msaaValue);

            // scene.AddLight(new Light(new Vector3(1f, -.9f, -1.2f)) { Intensity = Vector3.One });
            // scene.AddLight(new Light(new Vector3(0, -0.9f, -1.2f)) { Intensity = Vector3.One });
            // scene.AddLight(new Light(new Vector3(0, 1, -2.2f)) { Intensity = new Vector3(0, 0, 1) * 1 });
            //scene.AddLight(new Light(new Vector3(0, 2f, 0), new Vector3(1,1,0.4f)*100));
            //scene.AddLight(new Light(new Vector3(-2, -0.5f, 0), 20) { Intensity = new Vector3(0, 1, 0) });
            //scene.AddLight(new Light(new Vector3(0, 1f, -2f), new Vector3(1, 1, 0)*10));
            scene.AddLight(new SpotLight(camera.Position, new Vector3(0,0,5), 5f));

            scene.AddPrimitive(new Sphere(new Vector3(0, 0, 0), 1f, new Vector3(1f)) { PrimitiveName = "White Sphere", Reflectivity = 0.5f});
            scene.AddPrimitive(new Sphere(new Vector3(-1.5f, 1.2f, 1f), 0.7f, new Vector3(0, 0, 1f)) { PrimitiveName = "Blue Sphere", Reflectivity = 0.01f});
            scene.AddPrimitive(new Floor(new Vector3(0, 1, 0), -1f) { PrimitiveName = "Floor", Reflectivity = 0.5f});
            scene.AddPrimitive(new Plane(new Vector3(0, -1, 0), -5f) { PrimitiveName = "Roof" , Color = new Vector3(0, 0, 1)});
            scene.AddPrimitive(new Plane(new Vector3(0, 0, -1), -5f) { Color = new Vector3(1, 0, 0) });
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

            float vertStep = 1f / screen.height;


            if (MSAA != 1 && !camera.IsMoving)
            {
                DrawMSAA(screen, debugScreen);
            }
            else
            {
                y = 0;
                u = 0;
                v = 0;
                offset = 0;
                DrawNoMSAA(screen, debugScreen);
            }
        }

        void DrawMSAA(Surface screen, Surface debugScreen)
        {
            Vector3 subScreenPoint;
            Vector3 subColor = new Vector3(0);
            Vector3 finalColor = new Vector3(0);
            Ray ray;
            Vector3 screenPoint;
            Vector3 screenHorz = camera.TopRight - camera.TopLeft; //Horizontal vector of the screen
            Vector3 screenVert = camera.BottomLeft - camera.TopLeft; //Vertical vector of the screen
            float horzStep = 1f / screen.width;
            float vertStep = 1f / screen.height;

            for (int x = 0; x < screen.width; ++x, u += horzStep)
            {
                screenPoint = camera.TopLeft + u * screenHorz + v * screenVert; //Top left + u * horz + v * vert => screen point
                finalColor = new Vector3(0);
                for (float subY = 0; subY < msaaValue; subY++)
                    for (float subX = 0; subX < msaaValue; subX++)
                    {
                        subScreenPoint = new Vector3(screenPoint.X + (axisoffsetX * subX), screenPoint.Y + (axisoffsetY * subY), screenPoint.Z);
                        //subScreenPoint = screenPoint;
                        Vector3 dir = subScreenPoint - camera.Position;  //A vector from the camera to that screen point
                        ray = new Ray(dir.Normalized(), camera.Position);  //Create a primary ray from there

                        //foreach (Primitive p in scene.Primitives)
                        //   p.Intersect(ray);  //Calculate the intersection with all the primitives

                        //byte i = (byte)(1024 / (ray.Intsect.Distance * ray.Intsect.Distance));
                        
                        subColor = ray.GetColor(scene);

                        finalColor += subColor;
                        //Console.WriteLine("InLoop: " +subScreenPoint + " : " + subColor + " : " + finalColor);

                        //Draw some rays on the debug screen
                        if (subY == 0 && subX == 0 && y == (debugScreen.height >> 1) && x % 32 == 0)
                        {
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
                                0x00ff00);
                        }

                    }

                finalColor = finalColor / MSAA;
                //Console.WriteLine("OutLoop: " + finalColor);    
                screen.pixels[x + offset] = CreateColor(Clamp(finalColor));
            }

            if (y < screen.height - 1)
            {
                y += 1;
                u = 0;
                v += vertStep;
                offset += screen.width;
            }
            else
            {
                y = 0;
                u = 0;
                v = 0;
                offset = 0;
            }
        }

        void DrawNoMSAA(Surface screen, Surface debugScreen)
        {
            Ray ray;
            Vector3 screenPoint;
            Vector3 screenHorz = camera.TopRight - camera.TopLeft; //Horizontal vector of the screen
            Vector3 screenVert = camera.BottomLeft - camera.TopLeft; //Vertical vector of the screen
            float horzStep = 1f / screen.width;
            float vertStep = 1f / screen.height;
            float u = 0, v = 0;
            int offset = 0;
            for (int y2 = 0; y2 < screen.height; ++y2, u = 0, v += vertStep, offset += screen.width)
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
                        if (random.Next(10) == 0)
                            screen.pixels[x + offset] = CreateColor(Clamp(ray.GetStaticColor(scene) * Clamp(Vector3.Dot(ray.Direction, -ray.Intsect.Normal))));
                    }

                    //Draw some rays on the debug screen
                    if (y2 == (debugScreen.height >> 1) && x % 32 == 0)
                    {
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
                            0x00ff00);
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
