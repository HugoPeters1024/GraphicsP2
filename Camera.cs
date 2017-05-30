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
        double FOVd, FOVr, FOVcalc;
        bool isMoving;

        public Camera(Vector3 position, Vector3 direction)
        {
            width = 1f;
            height = 1f;
            FOVcalc = Math.PI / 180;
            FOVd = 90;
            FOVr = FOVd * FOVcalc;
            distance = (float)((width / 2.0) / Math.Tan(FOVr / 2.0));
            this.position = position;
            this.direction = Vector3.Normalize(direction);
        }

        public void DrawDebug(Surface screen)
        {
            DrawCircle(screen, position.X, position.Z, 0.1f, Vector3.One);
            screen.Line(TX(TopLeft.X,screen), TY(TopLeft.Z,screen), TX(TopRight.X, screen), TY(TopRight.Z, screen), 0xffffff);
        }

        public void Update()
        {
            if (KeyboardHandler.IsAnyKeyDown())
            {
                IsMoving = true;

                //Rotation control
                #region Rotation
                //rotate left
                if (KeyDown(Key.Left))
                {
                    Vector3 v = Center;
                    v -= 0.01f * Vector3.Normalize(TopRight - TopLeft);
                    direction = Vector3.Normalize(v - position);
                }

                //rotate right
                if (KeyDown(Key.Right))
                {
                    Vector3 v = Center;
                    v += 0.01f * Vector3.Normalize(TopRight - TopLeft);
                    direction = Vector3.Normalize(v - position);
                }

                //rotate up
                if (KeyDown(Key.Up) && direction.Y < 0.9f)
                {
                    Vector3 v = Center;
                    v -= 0.01f * Vector3.Normalize(BottomLeft - TopLeft);
                    direction = Vector3.Normalize(v - position);
                }

                //rotate down
                if (KeyDown(Key.Down) && direction.Y > -0.9f)
                {
                    Vector3 v = Center;
                    v += 0.01f * Vector3.Normalize(BottomLeft - TopLeft);
                    direction = Vector3.Normalize(v - position);
                }
                #endregion

                //Movement control
                #region Movement
                //move forwards
                if (KeyDown(Key.W))
                {
                    position.X += 0.05f * direction.X;
                    position.Z += 0.05f * direction.Z;
                }

                //move backwards
                if (KeyDown(Key.S))
                {
                    position.X -= 0.05f * direction.X;
                    position.Z -= 0.05f * direction.Z;
                }

                //move rightwards
                if (KeyDown(Key.D))
                {
                    position.X += 0.05f * direction.Z;
                    position.Z -= 0.05f * direction.X;
                }

                //move leftwards
                if (KeyDown(Key.A))
                {
                    position.X -= 0.05f * direction.Z;
                    position.Z += 0.05f * direction.X;
                }

                //move up
                if (KeyDown(Key.LShift))
                {
                    position.Y += 0.05f;
                }

                //move down
                if (KeyDown(Key.LControl))
                {
                    position.Y -= 0.05f;
                }
                #endregion

                //Manual FOV control
                #region Manual FOV
                if (KeyDown(Key.I))
                {
                    //decrease FOV
                    FOVd -= 5;
                    FOVr = FOVd * FOVcalc;
                    distance = (float)((width / 2.0) / Math.Tan(FOVr / 2.0));
                }

                if (KeyDown(Key.K))
                {
                    //increase FOV
                    FOVd += 5;
                    FOVr = FOVd * FOVcalc;
                    distance = (float)((width / 2.0) / Math.Tan(FOVr / 2.0));
                }
                #endregion
            }
            else
            {
                IsMoving = false;
            }

            //Other controls
            #region Others
            if (KeyDown(Key.L))
            {
                Console.WriteLine("Type desired FOV in degrees:");
                string s = Console.ReadLine();
                try
                {
                    FOVd = double.Parse(s);
                    FOVr = FOVd * FOVcalc;
                    distance = (float)((width / 2.0) / Math.Tan(FOVr / 2.0));
                    TOGGLE = true;
                    Console.WriteLine("FOV successfully set to: " + FOVd + " degrees");
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

            if(KeyDown(Key.T))
            {
                Console.WriteLine("Index : Primitives");
                for(int j = 0; j < RayTracer.Scene.Primitives.Count; j++)
                {
                    Console.Write(j);
                    Console.Write(" : ");
                    Console.WriteLine(RayTracer.Scene.Primitives[j].PrimitiveName);
                }
                Console.WriteLine("Type desired target's index number:");
                string s = Console.ReadLine();
                try
                {
                    int i = int.Parse(s);
                    if (i < 0 || i > RayTracer.Scene.Primitives.Count - 1)
                    {
                        Console.WriteLine("Error, input value not in range of list. Reset to default '0'");
                        TOGGLE = true;
                        direction = Vector3.Normalize(RayTracer.Scene.Primitives[0].Position - position);
                    }
                    else
                    {
                        Console.Write("Succes, target set to: ");
                        Console.WriteLine(RayTracer.Scene.Primitives[i].PrimitiveName);
                        TOGGLE = true;
                        direction = Vector3.Normalize(RayTracer.Scene.Primitives[i].Position - position);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            #endregion
        }

        #region Properties
        public Vector3 Center
        {
            get { return position + distance * direction; }
        }

        public Vector3 TopLeft
        {
            get
            {
                Vector3 v = Vector3.Cross(Vector3.UnitY, direction);
                return Center - width / 2 * v - Vector3.Cross(v, direction) * height / 2.0f;
            }
        }

        public Vector3 TopRight
        {
            get
            {
                Vector3 v = Vector3.Cross(Vector3.UnitY, direction);
                return Center + width / 2 * v - Vector3.Cross(v, direction) * height / 2.0f;
            }
        }

        public Vector3 BottomLeft
        {
            get
            {
                Vector3 v = Vector3.Cross(Vector3.UnitY, direction);
                return Center - width / 2 * v + Vector3.Cross(v, direction) * height / 2.0f;
            }
        }

        public Vector3 Position
        {
            get { return position; }
        }

        public Vector3 Direction
        {
            get { return direction; }
        }

        public bool IsMoving
        {
            get { return isMoving; }
            set { isMoving = value; }
        }
        #endregion
    }
}
