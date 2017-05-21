using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template
{
    static class FunctionWrapper
    {
        public const float EPS = 0.0001f;
        public const int MAX_DEPTH = 16;
        public const int GLOSS = 2;
        static Random rand = new Random();

        /// <summary>
        /// converts values of x from -range/2 ... range/2 to 0 ...640
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int TX(float x, Surface screen)
        {
            x += (Game.SCENE_WIDTH / 2);
            x *= screen.width / Game.SCENE_WIDTH; //Mulitply the unit line by the screen width over the range to get a pixel coordinate
            return (int)x;
        }

        /// <summary>
        /// converts values of y from -range/2 ... range/2 to 0 ... 400 (and negates y)
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int TY(float y, Surface screen)
        {
            y = -y; //Reverse y
            y += (Game.SCENE_HEIGHT / 2); //Correct again for the Cartesian coordinate system
            y *= screen.height / Game.SCENE_HEIGHT; //Mulitply the unit line by the screen height over the range to get a pixel coordinate
            return (int)y;
        }

        public static void DrawCircle(Surface s, float xin, float yin, float rin, Vector3 c)
        {
            int x0 = TX(xin, s);
            int y0 = TY(yin, s);
            int radius = (int)(rin * (s.width / Game.SCENE_WIDTH));
            int x = radius;
            int y = 0;
            int err = 0;
            int col = ConstructColor(c.X, c.Y, c.Z);

            while (x >= y)
            {
                s.Plot(x0 + x, y0 + y, col);
                s.Plot(x0 + y, y0 + x, col);
                s.Plot(x0 - y, y0 + x, col);
                s.Plot(x0 - x, y0 + y, col);
                s.Plot(x0 - x, y0 - y, col);
                s.Plot(x0 - y, y0 - x, col);
                s.Plot(x0 + y, y0 - x, col);
                s.Plot(x0 + x, y0 - y, col);

                y += 1;
                if (err <= 0)
                {
                    err += 2 * y + 1;
                }
                if (err > 0)
                {
                    x -= 1;
                    err -= 2 * x + 1;
                }
            }
        }

        public static int ConstructColor(float r, float g, float b)
        {
            return ((int)Clamp(r)*255) << 16 ^ ((int)Clamp(g)*255) << 8 ^ ((int)Clamp(b)*255);
        }

        public static float Clamp(float v, float min = 0f, float max = 1f)
        {
            if (v < min) { return min; }
            if (v > max) { return max; }
            return v;
        }

        public static Vector3 Clamp(Vector3 v, float min = 0f, float max = 1f)
        {
            v.X = Clamp(v.X, min, max);
            v.Y = Clamp(v.Y, min, max);
            v.Z = Clamp(v.Z, min, max);
            return v;
        }

        public static int CreateColor(Vector3 c)
        {
            return (int)(c.X * 255) << 16 ^ (int)(c.Y * 255) << 8 ^ (int)(c.Z * 255);
        }

        public static Random Rand
        {
            get { return rand; }
        }
    }
}
