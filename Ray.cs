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
            this.direction = Vector3.Normalize(direction);
            intsect = new Intersection(null, 5, this.direction);
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
                    return intsect.Primitive.Color * DirectIllumination(origin + direction * intsect.Distance, intsect.Normal, s);// * Clamp(Vector3.Dot(direction, intsect.Normal));
                else
                    return intsect.Primitive.Color * (1f - intsect.Primitive.Reflect); //TODO: +new reflected ray
            }
            return Vector3.Zero;
        }

        Vector3 DirectIllumination(Vector3 I, Vector3 N, Scene s) //Intersection Point, Normal
        {
            Vector3 color = Vector3.Zero;
            foreach (Light light in s.Lights)
            {
                Vector3 L = light.Origin - I;
                float dist = (float)Math.Sqrt(Vector3.Dot(L, L));
                L *= (1.0f / dist);
                if (IsVisible(I, L, dist, s))
                {
                    float attenuation = (1f / (dist * dist)) - EPS;
                    if (attenuation > 0)
                        color += light.Intensity * Vector3.Dot(N, L) * attenuation;
                }
            }
            return color;
        }        bool IsVisible(Vector3 I, Vector3 L, float dist, Scene s)
        {
            Ray shadowRay = new Ray(L, I + EPS * Vector3.Normalize(L));
            foreach(Primitive p in s.Primitives)
            {
                p.Intersect(shadowRay);
                if (shadowRay.intsect.Primitive != null && shadowRay.intsect.Distance < dist - 2 * EPS)
                    return false;
            }
            return true;
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
