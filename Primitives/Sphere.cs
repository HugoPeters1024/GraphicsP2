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
            DrawCircle(debugScreen, position.X, position.Z, radius, color);
            base.DrawDebug(debugScreen);
        }

        public override void Intersect(Ray ray)
        {
            Vector3 c = position - ray.Origin;
            float t = Vector3.Dot(c, ray.Direction);
            Vector3 q = c - t * ray.Direction;
            float p2 = Vector3.Dot(q, q);
            float r2 = radius * radius;
            if (p2 > r2)
                return;

            t -= (float)Math.Sqrt(r2 - p2);
            if (t < ray.Intsect.Distance && t > 0)
            {
                Vector3 point = ray.Origin + t * ray.Direction; //Point of contact on the circle
                ray.Intsect = new Intersection(this, t, point - position);
            }
        }
    }
}
