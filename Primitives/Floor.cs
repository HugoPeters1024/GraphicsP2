using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template
{
    class Floor : Plane
    {
        public Floor(Vector3 normal, float d) : base(normal, d)
        {
            this.PrimitiveName = "Floor";
        }

        public override void Intersect(Ray ray)
        {
            float t = -(Vector3.Dot(ray.Origin, normal) + d) / (Vector3.Dot(ray.Direction, normal));
            Vector3 coord = ray.Origin + t * ray.Direction;
            color = Vector3.Zero;
            if ((Math.Floor(coord.X * 4) + Math.Floor(coord.Z * 4)) % 2 == 0)
                color = Vector3.One;

            if (t > 0 && t < ray.Intsect.Distance)
                if (coord.X < -5f || coord.X > 5f || coord.Z < -5f || coord.Z > 5f)
                    return;
                else
                    ray.Intsect = new Intersection(this, t, normal);
        }
    }
}
