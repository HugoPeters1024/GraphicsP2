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
        static PFM skydome;

        // initialize
        public void Init()
        {
            //Screen size is defined in the template.cs
            viewScreen = new Surface(OpenTKApp.VIEW_WIDTH, OpenTKApp.VIEW_HEIGHT);
            debugScreen = new Surface(OpenTKApp.DEBUG_WIDTH, OpenTKApp.DEBUG_HEIGHT);
            rayTracer = new RayTracer(screen, debugScreen);

            /// 0 = Eucalyptus Grove, UC Berkeley
            // int f = 0;

            /// 1 = The Uffizi Gallery, Florence
            // int f = 1;

            /// 2 = St. Peter's Basilica, Rome (This option looks the best with our scene)
            int f = 2;
            
            skydome = new PFM("../../assets/" + f + ".pfm");
                       
            KeyboardHandler.Init();
            Debugger.DrawDebug();
        }

        public void Tick()
        { 
            KeyboardHandler.Update();
            screen.Clear(0);
            rayTracer.Draw(viewScreen, debugScreen);

            AddSurface(screen.width - viewScreen.width, 0, debugScreen);

            AddSurface(0, 0, viewScreen);
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