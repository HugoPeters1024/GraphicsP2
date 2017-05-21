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
        float radius;

        public SpotLight(Vector3 position, Vector3 direction, float radius) : base(position)
        {
            this.direction = Vector3.Normalize(direction);
            this.radius = radius;
        }

        public SpotLight(Vector3 position, Vector3 intensity, Vector3 direction, float radius) : base(position, intensity)
        {
            this.direction = Vector3.Normalize(direction);
            this.radius = radius;
        }

        public SpotLight(Vector3 position, float intensity, Vector3 direction, float radius) : base(position, intensity)
        {
            this.direction = Vector3.Normalize(direction);
            this.radius = radius;
        }

        public override void DrawDebug(Surface screen)
        {
            screen.Line(TX(Origin.X, screen), TY(Origin.Z, screen), TX(direction.X, screen), TY(direction.Z, screen), 0xffffff);
            base.DrawDebug(screen);
        }

        #region Properties
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public override Vector3 Intensity
        {
            get
            {
                return base.Intensity;
            }

            set
            {
                base.Intensity = value;
            }
        }
        #endregion
    }
}
