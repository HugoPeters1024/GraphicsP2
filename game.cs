using System;
using System.IO;

namespace template
{
    class Game
    {
        public static int VIEW_WIDTH = OpenTKApp.APP_WIDTH/2;
        public static int VIEW_HEIGHT = OpenTKApp.APP_HEIGHT;
        public static int DEBUG_WIDTH = OpenTKApp.APP_WIDTH/2;
        public static int DEBUG_HEIGHT = OpenTKApp.APP_HEIGHT;
        public static float SCENE_WIDTH = 10f;
        public static float SCENE_HEIGHT = SCENE_WIDTH;

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
            KeyboardHandler.Init();
        }

        public void Tick()
        { 
            KeyboardHandler.Update();
            screen.Clear(0);
            debugScreen.Clear(0);
            app.Draw(viewScreen, debugScreen);

            AddSurface(0, 0, viewScreen);
            AddSurface(screen.width - debugScreen.width, 0, debugScreen);
        }

        //Company surfaces by an offset and perform a memcopy
        public void AddSurface(int xs, int ys, Surface s)
        {
            for (int x = 0; x < s.width; ++x)
                for (int y = 0; y < s.height; ++y)
                    screen.pixels[xs + x + (ys + y) * screen.width] = s.pixels[x + s.width * y];
        }
    }

}// namespace Template