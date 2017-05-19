using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using static template.FunctionWrapper;
using static template.KeyboardHandler;

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
            this.direction = Vector3.Normalize(direction);
        }

        public void DrawDebug(Surface screen)
        {
            DrawCircle(screen, position.X, position.Z, 0.1f, Vector3.One);
            screen.Line(TX(TopLeft.X,screen), TY(TopLeft.Z,screen), TX(TopRight.X, screen), TY(TopRight.Z, screen), 0xffffff);
            //screen.Box(TX(TopLeft.X, screen), TY(TopLeft.Z - 0.02f, screen), TX(TopRight.X, screen), TY(TopRight.Z + 0.02f, screen), 0xffffff);
        }

        public void Update()
        {
            if (KeyDown(Key.Left))
            {
                //rotate left
                Vector3 v = Center;
                v -= 0.1f * Vector3.Normalize(TopRight - TopLeft);
                direction = Vector3.Normalize(v - position);
            }
            if (KeyDown(Key.Right))
            {
                //rotate right
                Vector3 v = Center;
                v += 0.1f * Vector3.Normalize(TopRight - TopLeft);
                direction = Vector3.Normalize(v - position);
            }
            if (KeyDown(Key.Up))
            {
                //rotate up
            }
            if (KeyDown(Key.Down))
            {
                //rotate down
            }
        }

        #region Properties
        public Vector3 Center
        {
            get { return position + distance * direction; }
        }

        public Vector3 TopLeft
        {
            get { return Center  - width/2 * Vector3.Cross(Vector3.UnitY, direction) + Vector3.UnitY*height/2.0f; }
        }

        public Vector3 TopRight
        {
            get { return Center + width / 2 * Vector3.Cross(Vector3.UnitY,direction ) + Vector3.UnitY * height / 2.0f; }
        }

        public Vector3 BottomLeft
        {
            get { return Center - width / 2 * Vector3.Cross(Vector3.UnitY,direction ) - Vector3.UnitY * height / 2.0f; }
        }

        public Vector3 Position
        {
            get { return position; }
        }
        #endregion
    }
}
