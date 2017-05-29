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
        public static bool DEBUGSWITCH = false;
        Vector3 direction;
        Vector3 origin;
        Intersection intsect;
        //Ray reflectionRay;

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
                    return new Ray(ReflectedDirection, origin + direction * (intsect.Distance - EPS)).GetColor(s, depth + 1); 

                return intsect.Primitive.Color * DirectIllumination(origin + direction * (intsect.Distance - EPS), intsect.Normal, s) *
                        (1f - intsect.Primitive.Reflectivity)
                        +
                        intsect.Primitive.Reflectivity *
                        (new Ray(ReflectedDirection, origin + direction * (intsect.Distance - EPS)).GetColor(s, depth + 1));
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
            {
                if (DEBUGSWITCH)
                {
                    if (depth == 0)
                    { 
                        DirectIllumination(origin + direction * (intsect.Distance-EPS), intsect.Normal, s); //Will add shadowrays to the debug buffer
                        Ray reflectionRay = new Ray(ReflectedDirection, origin + direction * (intsect.Distance - EPS)); // generate a reflection ray
                        reflectionRay.GetStaticColor(s, depth + 1); //get a length
                        Debugger.AddReflectedRay(reflectionRay); //add it to the buffer
                        Ray n = new Ray(intsect.Normal, origin + direction * intsect.Distance); //generate a normal
                        n.intsect.Distance = 1f; //make it lenght 1
                        Debugger.AddNormal(n); // add it to the buffer
                    }
                }
                return intsect.Primitive.Color;
            }
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
                    Vector3 L = spot.Origin - I;  //From the intersection to the spotlight
                    float NdotL = Vector3.Dot(N, L);
                    
                    if (NdotL > 0)
                    {
                        float L2 = Vector3.Dot(L, L);
                        float t = Vector3.Dot(L, spot.Direction);
                        Vector3 distVec = L - (t * spot.Direction);
                        float dist2 = Vector3.Dot(distVec, distVec);
                        float dist = (float)Math.Sqrt(L2);
                        L.Normalize();
                        if (IsVisible(I, L, (float)Math.Sqrt(L2), s))
                        {
                            if (dist > spot.GetRadius(t))
                                return color;
                            else
                            {
                                //Vector3 intensity = spot.Intensity * (Clamp(NdotL / dist2));
                                float dist3 = (float)Math.Sqrt(Vector3.Dot(distVec, distVec));
                                Vector3 intensity = spot.Intensity * (1-(dist3 / spot.GetRadius(t)));
                                //if (IsVisible(I, L, dist3, s))
                                {
                                    float attenuation = (1f / (L2)) - EPS;
                                    if (attenuation > 0)
                                        color = Clamp(color + intensity * NdotL * attenuation);
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
            Ray shadowRay = new Ray(L, I);
            foreach(Primitive p in s.Primitives)
            {
                p.Intersect(shadowRay);
                if (shadowRay.intsect.Distance < (dist - 2 * EPS))
                    return false;
            }
            shadowRay.intsect.Distance = dist;

            if (DEBUGSWITCH == true)
            {
                Debugger.AddShadowRay(shadowRay);
            }
            return true;
        }

        public void DrawDebug(Surface screen, int color, bool shadow = false)
        {
            if (shadow && intsect.Primitive == null)
            {
                intsect.Distance = 1f;
            }
            screen.Line(
                TX(origin.X, screen),
                TY(origin.Z, screen),
                TX(origin.X + direction.X * intsect.Distance, screen),
                TY(origin.Z + direction.Z * intsect.Distance, screen),
                color);
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

        public Vector3 ReflectedDirection
        {
            get { return direction - 2 * Vector3.Dot(direction, intsect.Normal) * intsect.Normal; }
        }
        #endregion
    }
}
