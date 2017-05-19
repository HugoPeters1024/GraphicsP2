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

        }

        public override void Intersect(Ray ray)
        {
            base.Intersect(ray);
        }
    }
}
