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
        Vector3[] screenPos;

        public Camera(Vector3 position, Vector3 direction)
        {
            this.position = position;
            this.direction = direction;
            screenPos = new Vector3[4]
            {
               new Vector3(-1f , -1f, 1f),
               new Vector3(1f , -1f, 1f),
               new Vector3(1f , -1f, -1f),
               new Vector3(-1f , -1f, -1f)
            };
        }

        public void DrawDebug(Surface screen)
        {
            screen.Box(TX(screenPos[0].X, screen), TY(screenPos[0].Y, screen), TX(screenPos[1].X, screen), TY(screenPos[1].Y, screen) + 5, 0x000000);
        }
    }
}
