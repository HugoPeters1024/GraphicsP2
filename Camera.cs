using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using static template.FunctionWrapper;

namespace template
{
    class Camera
    {
        Vector3 position;
        Vector3 direction;
        float distance, width, height;

        public Camera(Vector3 position, Vector3 direction)
        {
            width = 1f;
            height = 1f;
            distance = 0.6f;
            this.position = position;
            this.direction = direction;
        }

        public void DrawDebug(Surface screen)
        {
            DrawCircle(screen, position.X, position.Z, 0.1f, Vector3.One);
            screen.Box(TX(TopLeft.X, screen), TY(TopLeft.Z - 0.02f, screen), TX(TopRight.X, screen), TY(TopRight.Z + 0.02f, screen), 0xffffff);
        }

        #region Properties
        public Vector3 Center
        {
            get { return position + distance * direction; }
        }

        public Vector3 TopLeft
        {
            get { return Center + new Vector3(-width/2, height/2, 0); }
        }

        public Vector3 TopRight
        {
            get { return Center + new Vector3(width / 2, height / 2, 0); }
        }

        public Vector3 BottomLeft
        {
            get { return Center + new Vector3(-width / 2, -height / 2, 0); }
        }

        public Vector3 Position
        {
            get { return position; }
        }
        #endregion
    }
}
