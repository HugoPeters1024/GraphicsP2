using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template
{
    class Intersection
    {
        Primitive primitive;
        float distance;
        Vector3 normal;

        public Intersection(Primitive p, float d, Vector3 n)
        {
            primitive = p;
            distance = d;
            normal = Vector3.Normalize(n);
        }

        #region Properties
        public Primitive Primitive
        {
            get { return primitive; }
            set { primitive = value; }
        }

        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public Vector3 Normal
        {
            get { return normal; }
            set { normal = value;}
        }
        #endregion
    }
}
