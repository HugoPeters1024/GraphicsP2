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
        Vector3 normal;

        public Plane(Vector3 position, Vector3 normal) : base(position)
        {
            this.normal = normal;
        }

        public Plane(Vector3 position, Vector3 color, Vector3 normal) : base(position, color)
        {
            this.normal = normal;
        }
    }
}
