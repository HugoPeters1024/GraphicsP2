using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using static template.FunctionWrapper;

namespace template
{
    class SpotLight : Light
    {
        Vector3 direction;
        float radiusCalc;
        float radiusScalar = 100f;

        public SpotLight(Vector3 position, Vector3 target, float rScalar) : base(position)
        {
            this.direction = target - position;
            this.direction = Vector3.Normalize(direction);
            radiusScalar = rScalar;
            radiusCalc = rScalar * (float)Math.Cos(60);
        }

        public SpotLight(Vector3 position, Vector3 intensity, Vector3 target) : base(position, intensity)
        {
            this.direction = target - position;
            this.direction = Vector3.Normalize(direction);
            radiusCalc = radiusScalar * (float)Math.Cos(60);
        }

        public SpotLight(Vector3 position, float intensity, Vector3 target) : base(position, intensity)
        {
            this.direction = target - position;
            this.direction = Vector3.Normalize(direction);
            radiusCalc = radiusScalar * (float)Math.Cos(60);
        }

        public override void DrawDebug(Surface screen)
        {
            screen.Line(TX(Origin.X, screen), TY(Origin.Z, screen), TX(direction.X, screen), TY(direction.Z, screen), 0xffffff);
            base.DrawDebug(screen);
        }

        public float GetRadius(float dist)
        {
            return dist * radiusCalc;
        }

        public float GetIntensity(float dist, float t)
        {
            return Clamp(1 - (dist / GetRadius(t)));
        }

        #region Properties
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        
        public float RadiusScalar
        {
            get { return radiusScalar;}
            set { radiusScalar = value; }
        }
        #endregion
    }
}
