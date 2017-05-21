﻿using System;
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
            this.direction = direction;
            intsect = new Intersection(null, 1 << 8, this.direction);
            this.origin = origin;
        }

        public Vector3 GetColor(Scene s, int depth = 0)
        {
            if (depth > MAX_DEPTH)
                return Vector3.Zero;
            foreach (Primitive p in s.Primitives)
            {
                p.Intersect(this);
            }
            if (intsect.Primitive != null)
            {
                if (intsect.Primitive.Reflectivity == 0)
                    return intsect.Primitive.Color * DirectIllumination(origin + direction * intsect.Distance, intsect.Normal, s);// * Clamp(Vector3.Dot(direction, intsect.Normal));
                
                if (intsect.Primitive.Reflectivity == 1)
                    return (new Ray(ReflectedRay, origin + direction * (intsect.Distance + EPS)).GetColor(s, depth + 1));

                return intsect.Primitive.Color * DirectIllumination(origin + direction * intsect.Distance, intsect.Normal, s) *
                        (1f - intsect.Primitive.Reflectivity)
                        +
                        intsect.Primitive.Reflectivity *
                        (new Ray(ReflectedRay, origin + direction * (intsect.Distance+EPS)).GetColor(s, depth + 1));
            }
            return Vector3.Zero;
        }

        public Vector3 GetStaticColor(Scene s, int depth = 0)
        {
            if (depth > MAX_DEPTH)
                return Vector3.Zero;
            foreach (Primitive p in s.Primitives)
            {
                p.Intersect(this);
            }
            if (intsect.Primitive != null)
                return intsect.Primitive.Color;
            else
                return Vector3.Zero;
        }

        Vector3 DirectIllumination(Vector3 I, Vector3 N, Scene s) //Intersection Point, Normal
        {
            Vector3 color = Vector3.Zero;
            foreach (Light light in s.Lights)
            {
                if (light is SpotLight)
                {
                    SpotLight spot = light as SpotLight;
                    Vector3 c = spot.Origin - I;
                    float Ndotc = Vector3.Dot(N, c);
                    if (Ndotc > 0)
                    {
                        float c2 = Vector3.Dot(c, c);
                        float t = Vector3.Dot(c, spot.Direction);
                        Vector3 distVec = I - (spot.Origin - (t * spot.Direction));
                        float dist2 = Vector3.Dot(distVec, distVec);
                        float dist = (float)Math.Sqrt(dist2);
                        c.Normalize();
                        if (IsVisible(I, c, dist, s))
                        {
                            if (dist > spot.GetRadius(t))
                                return color;
                            else
                            {
                                Vector3 intensity = spot.Intensity;// * (dist / spot.GetRadius(t));
                                    float dist3 = (float)Math.Sqrt(c2);
                                if (IsVisible(I, c, dist3, s))
                                {
                                    float attenuation = (1f / (c2)) - EPS;
                                    if (attenuation > 0)
                                        color = Clamp(color + intensity * Ndotc * attenuation);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Vector3 L = light.Origin - I;
                    float NdotL = Vector3.Dot(N, L);
                    if (NdotL > 0)
                    {
                        float L2 = Vector3.Dot(L, L);
                        float dist = (float)Math.Sqrt(L2);
                        L.Normalize();
                        if (IsVisible(I, L, dist, s))
                        {
                            float attenuation = (1f / (L2)) - EPS;
                            if (attenuation > 0)
                                color = Clamp(color + light.Intensity * NdotL * attenuation);
                        }
                    }
                }
            }
            return color;
        }

        bool IsVisible(Vector3 I, Vector3 L, float dist, Scene s)
        {
            Ray shadowRay = new Ray(L, I + EPS * L);
            foreach(Primitive p in s.Primitives)
            {
                p.Intersect(shadowRay);
                if (shadowRay.intsect.Primitive != null && shadowRay.intsect.Distance < (dist - 2 * EPS))
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

        public Vector3 ReflectedRay
        {
            get { return Vector3.Normalize(direction - 2 * Vector3.Dot(direction, intsect.Normal) * intsect.Normal); }
        }
        #endregion
    }
}
