using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template
{
    class Ray
    {
        Vector3 direction;
        Vector3 origin;
        float dist;

        public Ray(Vector3 direction, Vector3 origin)
        {
            dist = 10f;
            this.direction = Vector3.Normalize(direction);
            this.origin = origin;
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

        public float Dist
        {
            get { return dist; }
            set { dist = value; }
        }
        #endregion
    }
}
