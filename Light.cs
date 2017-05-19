using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using static template.FunctionWrapper;

namespace template
{
    class Light
    {
        Vector3 position;
        Vector3 intensity;

        public Light(Vector3 position)
        {
            this.position = position;
            intensity = new Vector3(1f);
        }

        public Light(Vector3 position, Vector3 intensity)
        {
            this.position = position;
            this.intensity = intensity;
        }

        public Light(Vector3 position, float intensity)
        {
            this.position = position;
            this.intensity = new Vector3(intensity);
        }

        public void DrawDebug(Surface screen)
        {
            DrawCircle(screen, position.X, position.Z, 0.1f, Vector3.One);
        }


        #region Properties
        public Vector3 Intensity
        {
            get { return intensity; }
            set { intensity = value; }
        }

        public Vector3 Origin
        {
            get { return position; }
            set { position = value; }
        }
        #endregion
    }
}
