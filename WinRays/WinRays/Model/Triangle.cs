using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WinRays.Model
{
    class Triangle : SceneObject
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override double intersect(Ray ray)
        {
            throw new NotImplementedException();
        }

        public override Vector3D normal()
        {
            throw new NotImplementedException();
        }

        public override Vector3D normal(Vector3D intersectPoint)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
