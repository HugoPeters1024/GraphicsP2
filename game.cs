using System;
using System.IO;

namespace template
{
    class Game
    {
        static int VIEW_WIDTH = 640;
        static int VIEW_HEIGHT = 480;
        static int DEBUG_WIDTH = 200;
        static int DEBUG_HEIGHT = 200;

        // member variables
        public Surface screen;
        Surface debugScreen;
        Surface viewScreen;
        Application app;
        // initialize
        public void Init()
        {
            //Screen size is defined in the template.cs
            viewScreen = new Surface(VIEW_WIDTH, VIEW_HEIGHT);
            debugScreen = new Surface(DEBUG_WIDTH, DEBUG_HEIGHT);
            app = new Application();
        }

        public void Tick()
        {
            screen.Clear(0);
            debugScreen.Clear(0xffffff);
            app.Draw(viewScreen, debugScreen);

            AddSurface(0, 0, viewScreen);
            AddSurface(screen.width - debugScreen.width, 0, debugScreen);
        }

        public void AddSurface(int xs, int ys, Surface s)
        {
            for (int x = 0; x < s.width; ++x)
                for (int y = 0; y < s.height; ++y)
                    screen.pixels[xs + x + (ys + y) * screen.width] = s.pixels[x + s.width * y];
        }
    }

}// namespace Template