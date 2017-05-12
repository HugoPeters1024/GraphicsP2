using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template
{
    class Sphere : Primitive
    {
        float radius;

        public Sphere(Vector3 position, float radius) : base(position)
        {
            this.radius = radius;
        }

        public Sphere(Vector3 position, Vector3 color, float radius) : base(position, color)
        {
            this.radius = radius;
        }

        public override void DrawDebug(Surface debugScreen)
        {
            base.DrawDebug(debugScreen);
        }
    }
}
