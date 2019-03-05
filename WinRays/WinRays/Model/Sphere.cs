using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WinRays.Model
{
    class Sphere : SceneObject
    {
        private float sphereX;
        private float sphereY;
        private float sphereZ;
        private float radius;
        public Vector3D Center;
        private readonly double EPSILON = 1e-3;

        public Sphere(float sphereX, float sphereY, float sphereZ, float sphereRad)
        {
            this.sphereX = sphereX;
            this.sphereY = sphereY;
            this.sphereZ = sphereZ;
            radius = sphereRad;
            Center = new Vector3D(sphereX, sphereY, sphereZ);
        }

        public override double intersect(Ray TheRay)
        {

            double B_Term, C_Term, Discrim, T0, T1;

            // Calculate B and C values.. See maths text..
            B_Term = 2 * (TheRay.direction.X * (TheRay.origin.X - Center.X) +
                        TheRay.direction.Y * (TheRay.origin.Y - Center.Y) +
                        TheRay.direction.Z * (TheRay.origin.Z - Center.Z));

            C_Term = SQUARE(TheRay.origin.X - Center.X) +
                     SQUARE(TheRay.origin.Y - Center.Y) +
                     SQUARE(TheRay.origin.Z - Center.Z) - SQUARE(radius);

            Discrim = SQUARE(B_Term) - 4 * C_Term;

            Discrim = Math.Sqrt(Discrim);

            T0 = (-B_Term - Discrim) * 0.5;

            if (T0 > EPSILON)
                return T0;

            T1 = (-B_Term + Discrim) * 0.5;
            if (T1 > EPSILON)
                return T1;

            return 0.0;
        }

        private double SQUARE( double d )
        {
            return d * d;
        }

        public override Vector3D normal()
        {
            throw new NotImplementedException();
        }

        public override Vector3D normal(Vector3D intersectPoint)
        {
            Vector3D RetVal;

            RetVal = intersectPoint - Center;                    // Point 
            RetVal.Normalize();                                    // Normalise operator, need unit vector.

            return RetVal;
        }

    }
}
