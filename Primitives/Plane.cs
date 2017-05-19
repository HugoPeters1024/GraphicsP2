using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template
{
    class Plane : Primitive
    {
        protected Vector3 normal;
        protected float d;

        public Plane(Vector3 normal, float d) : base(normal)
        {
            this.normal = normal;
            this.d = d;
            this.PrimitiveName = "Plane";
        }

        public Plane(Vector3 normal, float d, Vector3 color) : base(normal, color)
        {
            this.normal = normal;
            this.d = d;
            this.PrimitiveName = "Plane";
        }

        public override void Intersect(Ray ray)
        {
            float t = -(Vector3.Dot(ray.Origin, normal) + d) / (Vector3.Dot(ray.Direction, normal));

            if (t > 0 && t < ray.Intsect.Distance)
                ray.Intsect = new Intersection(this, t, normal);
        }
    }
}
