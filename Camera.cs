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
            distance = 1f;
            this.position = position;
            this.direction = direction;
        }

        public void DrawDebug(Surface screen)
        {
            DrawCircle(screen, position.X, position.Y, 0.1f, Vector3.Zero);
            screen.Box(TX(center.X - width/2, screen), TY(center.Y - 0.01f, screen), TX(center.X + width/2, screen), TY(center.Y + 0.01f, screen) + 5, 0x000000);
            for(int i=0; i<=10; ++i)
            {
                screen.Line(TX(position.X, screen), TY(position.Y, screen), TX(center.X - width/2 + (i/10f) * width, screen), TY(center.Y, screen),  0xff0000);
            }
        }

        Vector3 center
        {
            get { return position + distance * direction; }
        }
    }
}
