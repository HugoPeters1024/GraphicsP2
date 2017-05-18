using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using static template.FunctionWrapper;

namespace template
{
    class Ray
    {
        Vector3 direction;
        Vector3 origin;
        Intersection intsect;

        public Ray(Vector3 direction, Vector3 origin)
        {
            intsect = new Intersection(null, 10f, Vector3.Zero);
            this.direction = Vector3.Normalize(direction);
            this.origin = origin;
        }

        public Vector3 GetColor(Scene s)
        {
            foreach (Primitive p in s.Primitives)
            {
                p.Intersect(this);
            }
            if (intsect.Primitive != null)
            {
                if (intsect.Primitive.Reflect == 0)
                    return intsect.Primitive.Color * Clamp(Vector3.Dot(direction, intsect.Normal));
                else
                    return intsect.Primitive.Color * (1f - intsect.Primitive.Reflect); //TODO: +new reflected ray
            }
            return Vector3.Zero;
        }


        #region Properties
        public Vector3 Origin
        {
            get{ return origin; }
        }

        public Vector3 Direction
        {
            get { return direction; }
        }

        public Intersection Intsect
        {
            get { return intsect; }
            set { intsect = value; }
        }
        #endregion
    }
}
