using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template
{
    class Camera
    {
        Vector3 position;
        Vector3 direction;
        Vector3[] screen;

        public Camera(Vector3 position, Vector3 direction)
        {
            this.position = position;
            this.direction = direction;
            screen = new Vector3[4]
            {
               new Vector3(-1f , -1f, 1),
               new Vector3(1f , -1f, 1),
               new Vector3(1f , 1f, 1),
               new Vector3(-1f , 1f, 1)
            };
        }
    }
}
