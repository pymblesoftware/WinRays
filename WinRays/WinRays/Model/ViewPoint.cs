using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WinRays.Model
{
    class ViewPoint
    {

        public Vector3D Right;
        public Vector3D Up;
        public Vector3D Location;
        public Vector3D Direction;

        public ViewPoint(Vector3D Dir, Vector3D Loc, Vector3D Right, Vector3D Up)
        {
            Direction = Dir;
            Location = Loc;
            this.Right = Right;
            this.Up = Up;
        }

        public ViewPoint()
        {

        }
    }
}
