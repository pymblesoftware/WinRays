using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Drawing;


namespace WinRays.Model
{
    class LightSource : SceneObject
    {

        public Vector3D Center;
        private readonly double EPSILON = 1e-3;

        public override Vector3D normal()
        {
            return new Vector3D(0,0,0);
        }

        
        public override double intersect(Ray ray)
        {
            double RetVal = 0.0;
            return RetVal;
        }

        public LightSource(Vector3D origin)
        {
            Center = origin;
            properties = new Surface(1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
        }

        public void SetColour(Color color)
        {
            properties.Colour = color;
        }

        public Color getColour(SceneObject ClosestObj, Ray LightRay, double DistanceT, List<SceneObject> sceneList)
        {
            Color RetVal = Color.FromArgb(0, 0, 0);

            ////////////////////////////////////////////////////////////////
            // Iterate thru the scene objects list looking for objects 
            // between the object passed in and the light source..
            // Shadow .. black..	

            int pos;
            pos = 0;

            SceneObject CurrObject = null;

            if (sceneList.Count != 0)                                            /// Not an empty scene..
            {
                int currObjIndex = 0;
                // Start at the begining.

                CurrObject = sceneList.ElementAt(currObjIndex);

                bool EndOfList = false;
                while (!EndOfList)
                {
                    double InVal;

                    if (CurrObject != null)                                         // Valid object ?	
                    {

                        //
                        InVal = CurrObject.intersect(LightRay);                // Intersects ?
                        if ((InVal > (EPSILON)) && (InVal < DistanceT))      // Closer ?
                        {
                            if (CurrObject != ClosestObj)
                                return RetVal;                                  // Darkness.
                        }
                    }

                    // Not at end ? Then get the next object.

                    if ( currObjIndex < sceneList.Count - 1)
                    {
                        currObjIndex++;
                        CurrObject = sceneList.ElementAt(currObjIndex);
                    }
                    else
                    {
                        EndOfList = true;
                    }
                }
            }
            return properties.Colour;
        }
        public double MakeLightRay(Vector3D point, Ray lightRay)
        {
            double Distance;

            lightRay.origin = point;
            lightRay.direction = Center - point;
            Distance = lightRay.direction.Length;
            lightRay.direction.Normalize();                  // Normalise operator.

            return Distance;
        }

        public override Vector3D normal(Vector3D intersectPoint)
        {
            throw new NotImplementedException();
        }

    }
}
