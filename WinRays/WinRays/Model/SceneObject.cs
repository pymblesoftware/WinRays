using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Drawing;

namespace WinRays.Model
{


    abstract class SceneObject
    {
        public Surface properties;
        public Color pigment;

        abstract public Vector3D normal();
        abstract public double intersect(Ray ray);
        abstract public Vector3D normal(Vector3D intersectPoint);
    }
}
