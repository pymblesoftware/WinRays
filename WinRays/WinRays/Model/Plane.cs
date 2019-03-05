using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WinRays.Model
{
    class Plane : SceneObject
    {
        private double A;
        private double B;
        private double C;
        private double Distance;
        private readonly double EPSILON = 1e-3;


        public Plane(int v1, int v2, int v3, int v4)
        {
            A = v1;
            B = v2;
            C = v3;
            Distance = v4;
        }

        public Plane()
        {
            A = B = C = Distance = 0.0;
        }

        public override double intersect(Ray TheRay)
        {
            double Result;
            double DotProdDir;
            double DotProdOrig;

            Vector3D Norm = normal();                    // Normal.
            Norm.Normalize();                                      // Normalise vector.

            DotProdDir = Vector3D.DotProduct(Norm, TheRay.direction);       // Dot product..
            if (DotProdDir <= EPSILON)
            {
                // Console.WriteLine("(A) Returning 0.0 from plane intersect");
                return 0.0;
            }
            DotProdOrig = Vector3D.DotProduct(Norm, TheRay.origin);         // Dot product.
            DotProdOrig += Distance;

            // Check for intersection behind ray origin.	
            Result = DotProdOrig / DotProdDir;
            if (Result < 0.0)
            {
//                Console.WriteLine("(B) Returning 0.0 from plane intersect");
                return 0.0;
            }

            // Console.WriteLine("Returning {0} from plane intersect", Result.ToString());
            return Result;
        }

        public override Vector3D normal()
        {
            return new Vector3D(A, B, C);
        }

        //////////////////////////////////////////////////////////////////////
        // The normal or up vector of a flat surface is the same anywhere along
        // that flat surface. The input args are ignored and the normal is taken
        // from the planes definition.

        public override Vector3D normal(Vector3D intersectPoint)
        {
            return new Vector3D(A, B, C);
        }
    }
}
