using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace template
{
    class Scene
    {
        List<Primitive> primitives;
        List<Light> lights;

        public Scene()
        {
            primitives = new List<Primitive>();
            lights = new List<Light>();
        }

        public void AddPrimitive(Primitive p)
        {
            primitives.Add(p);
        }

        public void AddLight(Light l)
        {
            lights.Add(l);
        }

        public void DrawDebug(Surface debugScreen)
        {
            foreach (Primitive p in primitives)
            {
                p.DrawDebug(debugScreen);
            }

            foreach (Light l in lights)
            {
                l.DrawDebug(debugScreen);
                Console.WriteLine("Drawing a light!");
            }
        }

        #region Properties
        public List<Primitive> Primitives
        {
            get { return primitives; }
        }

        public List<Light> Lights
        {
            get { return lights; }
        }
        #endregion
    }
}
