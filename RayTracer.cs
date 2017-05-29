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

        int msaa;
        int msaaValue;
        float msaaFactor;
        float axisoffsetX, axisoffsetY;
        int yoffset;
        int yjump;
        SpotLight camLight;

        float u = 0, v = 0;
        int offset = 0;

        public RayTracer(Surface screen, Surface debugScreen)
        {
            camera = new Camera(new Vector3(0, 0, -3), new Vector3(0, 0, 1f));
            scene = new Scene();
            random = new Random();
            MSAA = 1;
            yoffset = 0;
            msaaValue = (int)Math.Sqrt(MSAA);

            //scene.AddLight(new Light(new Vector3(-2, -0.5f, 0), 20) { Intensity = new Vector3(0, 1, 0) });
            //scene.AddLight(new Light(new Vector3(0, 1f, -2f), new Vector3(1, 0.5f, 1) * 10));
            //scene.AddLight(new Light(new Vector3(-2, 1f, -2f), new Vector3(1, 0.5f, 1) * 10));
            //scene.AddLight(new Light(new Vector3(2, 1f, -2f), new Vector3(1, 0.5f, 1) * 10));
            //scene.AddLight(new Light(new Vector3(0, 2, 1), new Vector3(1, 1, 1)*10));
            //scene.AddLight(new SpotLight(camera.Position, 2f, new Vector3(0, 1f, 2)));
            camLight = new SpotLight(camera.Position, 5f, Vector3.UnitZ);
            scene.AddLight(camLight);

            scene.AddPrimitive(new Sphere(new Vector3(1.1f, 0, 0), 1f, new Vector3(1f)) { PrimitiveName = "Reflective Sphere", Reflectivity = 1f });
            scene.AddPrimitive(new Sphere(new Vector3(-1.1f, 0, 0), 1f, new Vector3(1f, 0.5f, 0.5f)) { PrimitiveName = "White Sphere", Reflectivity = 0f });
            //scene.AddPrimitive(new Sphere(new Vector3(-1, 0, 0), 1f, new Vector3(0, 0, 1f)) { PrimitiveName = "Blue Sphere"});

            scene.AddPrimitive(new Floor(new Vector3(0, 1, 0), -1f) { PrimitiveName = "Floor", Reflectivity = 0.5f});
            scene.AddPrimitive(new Plane(new Vector3(0, -1, 0), -5f) { PrimitiveName = "Roof" , Color = new Vector3(0, 0, 1)});
            scene.AddPrimitive(new Plane(new Vector3(0, 0, -1), -5f) { Color = new Vector3(1, 0, 0) });
            scene.AddPrimitive(new Plane(new Vector3(0, 0, 1), -5) { Color = new Vector3(0, 1, 1) });

            Debugger.Init(debugScreen, scene, camera);
        }

        public void Draw(Surface screen, Surface debugScreen)
        {
            camLight.Origin = camera.Position + new Vector3(0, 1, 0);
            //camLight.Direction = Vector3.Normalize(camera.Position - camera.Center);
            camera.Update();
            if (!camera.IsMoving)
            {
                DrawMSAA(screen, debugScreen);
            }
            else
            {
                yoffset = 0;
                u = 0;
                v = 0;
                offset = 0;
                MSAA = 1;
                DrawNoMSAA(screen, debugScreen);
            }
        }

        void DrawMSAA(Surface screen, Surface debugScreen)
        {
            if (MSAA == 64) return;
            Vector3 subScreenPoint;
            Vector3 finalColor = new Vector3(0);
            Ray ray = new Ray(Vector3.UnitX, Vector3.Zero);
            Vector3 screenPoint;
            Vector3 dir;
            Vector3 screenHorz = camera.TopRight - camera.TopLeft; //Horizontal vector of the screen
            Vector3 screenVert = camera.BottomLeft - camera.TopLeft; //Vertical vector of the screen
            float horzStep = 1f / screen.width;
            float vertStep = 1f / screen.height;
            float msX = 0;
            float msY = 0;

            for(int y = yoffset; y<(yoffset + yjump); ++y, u = 0, v+=vertStep, offset += screen.width)
                for (int x = 0; x < screen.width; ++x, u += horzStep)
                {
                    screenPoint = camera.TopLeft + u * screenHorz + v * screenVert; //Top left + u * horz + v * vert => screen point
                    dir = screenPoint - camera.Position;
                    finalColor = new Vector3(0);
                    msX = 0;
                    msY = 0;
                    for (int subY = 0; subY < msaaValue; subY++, msX = 0, msY += axisoffsetY)
                        for (int subX = 0; subX < msaaValue; subX++, msX += axisoffsetX)
                        {
                            subScreenPoint = new Vector3(screenPoint.X + msX, screenPoint.Y + msY, screenPoint.Z);
                            //subScreenPoint = screenPoint;
                            dir = subScreenPoint - camera.Position;  //A vector from the camera to that screen point
                            ray = new Ray(dir.Normalized(), camera.Position);  //Create a primary ray from there

                            //foreach (Primitive p in scene.Primitives)
                            //   p.Intersect(ray);  //Calculate the intersection with all the primitives

                            //byte i = (byte)(1024 / (ray.Intsect.Distance * ray.Intsect.Distance));
                        
                            finalColor += ray.GetColor(scene);
                            //Console.WriteLine("InLoop: " +subScreenPoint + " : " + subColor + " : " + finalColor);

                            //Draw some rays on the debug screen

                        }

                    screen.pixels[x + offset] = CreateColor(finalColor * msaaFactor);
                    
                }
            screen.Line(0, yoffset + yjump, screen.width, yoffset + yjump, 0xff00ff);
            if (screen.height - yoffset > 64)
                screen.Print("MSAAx" + MSAA.ToString(), screen.width / 2 - 20, yoffset + yjump + 2, 0xffffff);

            if (yoffset < screen.height - yjump)
            {
                yoffset += yjump;
            }
            else
            {
                yoffset = 0;
                u = 0;
                v = 0;
                offset = 0;
                if (MSAA < 64)
                    MSAA = MSAA * 4;
            }
        }

        public void DrawNoMSAA(Surface screen, Surface debugScreen)
        {
            Ray ray;
            Vector3 screenPoint;
            Vector3 screenHorz = camera.TopRight - camera.TopLeft; //Horizontal vector of the screen
            Vector3 screenVert = camera.BottomLeft - camera.TopLeft; //Vertical vector of the screen
            float horzStep = 1f / screen.width;
            float vertStep = 1f / screen.height;
            float u = 0, v = 0;
            int offset = 0;
            for (int y = 0; y < screen.height; ++y, u = 0, v += vertStep, offset += screen.width)
            {
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
                        if (random.Next(10) == 0 || y == screen.height >> 1)
                            screen.pixels[x + offset] = CreateColor(Clamp(ray.GetStaticColor(scene) * Vector3.Dot(ray.Direction, -ray.Intsect.Normal)));
                    }


                    Ray.DEBUGSWITCH = false;
                    //Draw some rays on the debug screen
                    if (y == screen.height >> 1 && x % 32 == 0)
                    {
                        Ray.DEBUGSWITCH = true;
                        Debugger.AddPrimaryRay(ray);
                    }
                }
            }
            Debugger.SwapBuffers();
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

        public int MSAA
        {
            get { return msaa; }
            set { msaa = value;
                msaaValue = (int)Math.Sqrt(msaa);
                yjump = 64 / msaaValue;
                msaaFactor = 1f / msaa;
                axisoffsetX = 1f / (OpenTKApp.VIEW_WIDTH * msaaValue);
                axisoffsetY = 1f / (OpenTKApp.VIEW_HEIGHT * msaaValue);
            }
        }
        #endregion
    }
}
