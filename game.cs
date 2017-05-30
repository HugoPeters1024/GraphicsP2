using System;
using System.IO;

namespace template
{
    class Game
    {
        public static float SCENE_WIDTH = 10f;
        public static float SCENE_HEIGHT = SCENE_WIDTH;

        // member variables
        public Surface screen;
        Surface debugScreen;
        Surface viewScreen;
        RayTracer rayTracer;
        Random r;

        static PFM skydome;
        /*
        static Surface skydome;
        static int[,] skydomeArray;
        */

        // initialize
        public void Init()
        {
            //Screen size is defined in the template.cs
            viewScreen = new Surface(OpenTKApp.VIEW_WIDTH, OpenTKApp.VIEW_HEIGHT);
            debugScreen = new Surface(OpenTKApp.DEBUG_WIDTH, OpenTKApp.DEBUG_HEIGHT);
            rayTracer = new RayTracer(screen, debugScreen);
            r = new Random();
            int f = r.Next(0, 3);

            /// 0 = Eucalyptus Grove, UC Berkeley
            // f = 0;

            /// 1 = The Uffizi Gallery, Florence
            // f = 1;

            /// 2 = St. Peter's Basilica, Rome
            // f = 2;

            //Console.WriteLine(f);
            skydome = new PFM("../../assets/" + f + ".pfm");
                       
            KeyboardHandler.Init();
            Debugger.DrawDebug();
        }

        public void Tick()
        { 
            KeyboardHandler.Update();
            screen.Clear(0);
            rayTracer.Draw(viewScreen, debugScreen);

            AddSurface(0, 0, viewScreen);
            AddSurface(screen.width - debugScreen.width, 0, debugScreen);
        }

        //Company surfaces by an offset and perform a memcopy
        public void AddSurface(int xs, int ys, Surface s)
        {
            int offset = ys * screen.width;
            for (int y = 0; y < s.height; ++y, offset += screen.width)
                for(int x=0; x<s.height; ++x)
                    screen.pixels[xs + x + offset] = s.pixels[x + s.width * y];
        }

        public static PFM Skydome
        {
            get { return skydome; }
        }

        /*
        #region Properties
        public static int[,] SkydomeArray
        {
            get { return skydomeArray; }
        }

        public static Surface Skydome
        {
            get { return skydome; }
        }
        #endregion
        */
    }

}// namespace Template