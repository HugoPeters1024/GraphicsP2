using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template
{
    abstract class Primitive
    {
        protected Vector3 position;
        protected Vector3 color;

        /// <summary>
        /// Constructor class for primitives
        /// </summary>
        /// <param name="position">The world position for this primitive</param>
        public Primitive(Vector3 position)
        {
            this.position = position;
            color = new Vector3(1f);
        }

        /// <summary>
        /// Constructor class for primitives
        /// </summary>
        /// <param name="position">The world position for this primitive</param>
        /// <param name="color">The color for this primitive</param>
        public Primitive(Vector3 position, Vector3 color)
        {
            this.position = position;
            this.color = color;
        }

        public virtual void DrawDebug(Surface debugScreen)
        {

        }

        public virtual void Intersect(Ray ray)
        {

        }
    }
}
