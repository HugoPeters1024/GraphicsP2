using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using static template.FunctionWrapper;

namespace template
{
    class Sphere : Primitive
    {
        float radius;

        public Sphere(Vector3 position, float radius) : base(position)
        {
            this.radius = radius;
        }

        public Sphere(Vector3 position, float radius, Vector3 color) : base(position, color)
        {
            this.radius = radius;
        }

        public override void DrawDebug(Surface debugScreen)
        {
            debugScreen.Print("Ello", 20, 40, 0x000000);
            DrawCircle(debugScreen, position.X, position.Y, radius, color);
            base.DrawDebug(debugScreen);
        }
    }
}
