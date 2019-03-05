using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRays.Model;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace WinRays.Controllers
{
    class Renderer
    {
        private readonly double EPSILON = 1e-3;
        private readonly int MAX_RECURSE_LEVEL = 15;
        List<SceneObject> sceneObjects;
        List<LightSource> lightSources;
        ViewPoint Camera;  // (Direction, Location, Right, Up );
        private WriteableBitmap img;
        private int width;
        private int height;
        private int RecurseLevel = 0;
        private double nWidth;
        private double nHeight;

        public void emptyScene()
        {
            sceneObjects.Clear();
            lightSources.Clear();
        }

        #region methods to make objects from parsed input

        public void makePlane()
        {

        }

        public void makeSphere()
        {

        }
        #endregion

        #region parsing methods

        // Read triangles from Direct3D .x file.
        public void readDotXFile()
        {

        }

        // Should be parsing not using the default world. Later.
        public void parseInput()
        {

        }
        #endregion

        public void makeDefaultWorld()
        {

            // TODO: Should be using the 'make' methods above. 
            // Red
            float SphereX = 50.0f;
            float SphereY = 0.0f;
            float SphereZ = 0.0f;
            float SphereRad = 25.0f;
            SceneObject TestObj = new Sphere(SphereX, SphereY, SphereZ, SphereRad);
            TestObj.pigment = Color.FromArgb( 220, 100, 100);
            TestObj.properties = new Surface(0.3, 0.7, 2.0, 0.9, 50.0, 0.065);


            // Clear / Blue
            SphereX = -37.5f;
            SphereY = -15.0f;
            SphereZ = -15.5f;
            SphereRad = 15.0f;
            SceneObject TestObj2 = new Sphere(SphereX, SphereY, SphereZ, SphereRad);
            TestObj2.pigment = Color.FromArgb(120,200,220);
            TestObj2.properties = new Surface(0.3, 0.7, 20.0, 0.9, 100.0, 0.75);

            // Yellow
            SphereX = 5.5f;
            SphereY = -42.5f;
            SphereZ = 5.0f;
            SphereRad = 17.5f;
            SceneObject TestObj3 = new Sphere(SphereX, SphereY, SphereZ, SphereRad);
            TestObj3.pigment = Color.FromArgb( 229, 220, 0 );
            TestObj3.properties = new Surface(0.9, 0.8, 20.0, 0.9, 0.85, 0.0);

            // DEBUG planes..
            SceneObject Floor = new Plane(0, 1, 0, -50);
            Floor.pigment = Color.FromArgb(150, 175, 150 );
            Floor.properties = new Surface(0.3, 0.7, 20.0, 0.9, 100.0, 0.65);
            LightSource Light;

            Light = new LightSource(new Vector3D(65, -50, -100));
            Light.properties = new Surface(1.0,1.0,1.0,1.0,1.0,1.0);
            Light.pigment = Color.FromArgb(255,255,255);

            // DEBUG code..
            sceneObjects.Add(TestObj);
            sceneObjects.Add(TestObj2);
            sceneObjects.Add(TestObj3);
            sceneObjects.Add(Floor);

            lightSources.Add(Light);
        }


        public Renderer(WriteableBitmap image )
        {
            img = image;

            sceneObjects = new List<SceneObject>();
            lightSources = new List<LightSource>();

            // Debug
            makeDefaultWorld();
        }



        public void setUpCamera(double aspectRatio)
        {
            // Create a camera or eye point.
            Vector3D Direction = new Vector3D( 0.0f, -0.05f, 1.0f );
            Vector3D Location = new Vector3D( 0.0f, -5.0f, -110.0f );
            Vector3D Right = new Vector3D( 1.0f, 0.0f, 0.0f );
            Vector3D Up = new Vector3D( 0.0f, aspectRatio, 0.0f );
            Camera = new ViewPoint(Direction, Location, Right, Up );
        }


        public void render(double width, double height)
        {
            //time_t StartTime = time(NULL);              // Start timer..
            long RendTime = 0;
        
            /////////////////////////////////////////////////////////////////////
            //	Set rendering dimentions by probing the window client area.

            nWidth = width;
            nHeight = height;
            double AspectRatio = nWidth / nHeight;

            bool WasEmpty = false;

            // Empty scene so create a default one for debug or demo.	
            if (sceneObjects.Count == 0 )
            {
                makeDefaultWorld();
                WasEmpty = true;
            }

            // Create a camera or eye point.
            setUpCamera(AspectRatio);

            //////////////////////////////////////////////////////////////////////////////
            //	Heart of the ray tracer..

            // Create image..
           

            for ( int row = 0; row < nHeight; row++)
                for ( int col = 0; col < nWidth; col++)
                    drawImage(col, row, Color.FromArgb(255, 255, 255) );


            // Debugging..
            //            return;

            Console.WriteLine("Starting");

            for (int row = 0; row < nHeight; row++)
            {
                for (int col = 0; col < nWidth; col++)
                {
                    Console.Write(".");
                    RecurseLevel = 0;                                   // Number of calls to Trace().
                    Ray TracedRay = new Ray();                                      // The Ray to trace.
                    Color Pixel = new Color();                                      /// The colour to set.
                    MakeRay(row, col, TracedRay, Camera);               // Create Starting view point.
                    if(trace(TracedRay, Pixel) )                            // Do the actual tracing of the ray.
                    {
                        Console.Write("t");
                        setColour(col, row, Color.Blue);
                    }
                    else
                    {
                        setColour(col, row, Color.Red);
                        Console.Write("f");
                    }
                    //    setColour(col, row, Pixel); // Set the result.
                }
                Console.WriteLine("");
            }
            Console.WriteLine("Finished");

        }

        private void MakeRay(int X, int Y, Ray TheRay, ViewPoint EyePos)
        {
            Vector3D Temp1, Temp2;
            double ScaledX, ScaledY;    // Screen co ordinates to converted to world.

            // Should Range between -0.5 to 0.5..
            ScaledX = ((double)X - (double)nWidth / 2.0) / (double)nWidth;
            ScaledY = ((double)Y - (double)nHeight / 2.0) / (double)nHeight;

            // Debug code...
//            assert((ScaledX <= 5.0) && (ScaledY <= 5.0));                   // Bound from above.
//            assert((ScaledX >= -5.0) && (ScaledY >= -5.0));             // Bound from below.

            // Work out direction...
            Temp1 = EyePos.Up * ScaledX;
            Temp2 = EyePos.Right * ScaledY;
            TheRay.direction = Temp1 + Temp2;
            TheRay.direction = EyePos.Direction + TheRay.direction;
            TheRay.direction.Normalize();                                                  // Normalise..
            TheRay.origin = EyePos.Location;                                    // Origin at View point.

        }


        public bool trace(Ray TheRay, Color Background)
        {
            bool intersected = false;
            Color RgbVal = Background;                                            // Default: Background colour ..

            if (RecurseLevel > MAX_RECURSE_LEVEL)
                return false;

            SceneObject ClosestObject = null;
            bool Intersected = false;
            // For every object, check for intersection.
            //	Save the closest objtect..
            if (sceneObjects.Count == 0)                                     // Empty scene..
            {
                return false;
            }

            SceneObject temp = sceneObjects.ElementAt(0);
            int currObjectIndex = 0;
            double ClosestTime = 999999.99;

            bool EndOfList = false;
            while (!EndOfList)
            {
                double InVal;

                InVal = temp.intersect(TheRay);                // Intersects ?
                if ((InVal > EPSILON) && (InVal < ClosestTime))     // Closer ?
                {
                    ClosestObject = temp;                           // Save..
                    ClosestTime = InVal;
                    Intersected = true;
                }

                currObjectIndex++;
                if( currObjectIndex < sceneObjects.Count )
                {
                    temp = sceneObjects.ElementAt(currObjectIndex);
                }
                else
                {
                    EndOfList = true;
                }
            }

            if (Intersected)                                                // We hit something....
            {
                Vector3D Normal;
                double NormalDir;

                Vector3D IntersectPoint = new Vector3D(ClosestTime * TheRay.direction.X +TheRay.origin.X,
								ClosestTime * TheRay.direction.Y + TheRay.origin.Y,
								ClosestTime * TheRay.direction.Z + TheRay.origin.Z
        							  );

                Normal = ClosestObject.normal(IntersectPoint);
                NormalDir = Vector3D.DotProduct( Normal, TheRay.direction );                  // Dot product of Normal and Ray.
                if (NormalDir > 0.0f)
                    Normal.Normalize();

                // Debug 
                //Background = Color.Crimson;
                
                shade(ClosestObject, TheRay, Normal, IntersectPoint, RgbVal);
            }

            return intersected;
        }

        public void shade(SceneObject ClosestObj, Ray TheRay, Vector3D Normal, Vector3D Point, Color col)
        {
                // If no intersection just return. A small optimisation.
                if (ClosestObj == null)
                    return;

                double K, Ambient, Diffuse, Specular, DistanceT;
                Ray LightRay = new Ray();
                Ray ReflectedRay = new Ray();
                double Red, Green, Blue;    // Working RGB values.. need to be onverted to a CColour later.	

                int NumLights;          // 

                LightSource Light;
                K = Vector3D.DotProduct(TheRay.direction, Normal );                  /// Dot product of the ray and the objects normal.
                K *= 2.0;

                // Reflect ray (obviousily) starts at the intersection point.
                ReflectedRay.origin = Point;

                ReflectedRay.direction = new Vector3D(K * Normal.X + TheRay.direction.X,
                                                   K * Normal.Y + TheRay.direction.Y,
                                                   K * Normal.Z + TheRay.direction.Z
                                                );
                Ambient = ClosestObj.properties.Ambient;
                Red = ClosestObj.properties.Colour.R * Ambient;
                Green = ClosestObj.properties.Colour.G * Ambient;
                Blue = ClosestObj.properties.Colour.B * Ambient;

                // Iterate thru a Light list. 
                // DEBUG for now use a bogus one.

                Light = new LightSource( new Vector3D(65, -50, -100));
                Light.SetColour(Color.FromArgb(255, 255, 255));

                NumLights = lightSources.Count();


                if (Light != null)
                {
                    Color LightColour;
                    DistanceT = Light.MakeLightRay(Point, LightRay);

                    if (RecurseLevel == 0)
                    {
                        LightColour = Light.getColour(ClosestObj, LightRay, DistanceT, sceneObjects);
                    }
                    else
                        LightColour = Light.properties.Colour;

                    if ((LightColour.R == 0.0) || (LightColour.G == 0.0) || (LightColour.B == 0.0))
                    {
                        col = Color.FromArgb(0, 0, 0);
                        return;
                    }
                    else
                    {
                        Diffuse = Vector3D.DotProduct(Normal, LightRay.direction);                                              // Dot Product 

                        if ((Diffuse > 0.0) && (ClosestObj.properties.Diffuse > 0.0))
                        {
                        
                            Diffuse = Math.Pow(Diffuse, ClosestObj.properties.Brilliance) * ClosestObj.properties.Diffuse;
                            Red += LightColour.R * ClosestObj.properties.Colour.R * Diffuse;
                            Green += LightColour.G * ClosestObj.properties.Colour.G * Diffuse;
                            Blue += LightColour.B * ClosestObj.properties.Colour.B * Diffuse;
                        }

                        ReflectedRay.direction.Normalize();    //
                        Specular = Vector3D.DotProduct(ReflectedRay.direction, LightRay.direction);                         // Dot product..


                        if ((Specular > 0.0) && (ClosestObj.properties.Specular > 0.0))
                        {
                            Specular = Math.Pow(Specular, ClosestObj.properties.Roughness) * ClosestObj.properties.Roughness;
                            Red += LightColour.R  * Specular;
                            Green += LightColour.G * Specular;
                            Blue += LightColour.B * Specular;
                        }

                        // Reflection ..
                        K = ClosestObj.properties.Reflection;
                        if (K > 0.0)
                        {
                            Color NewColour = new Color();
                            RecurseLevel++;

                            trace(ReflectedRay, NewColour);     // Recursive..
                            Red += NewColour.R * K;
                            Green += NewColour.G * K;
                            Blue += NewColour.B * K;

                            RecurseLevel--;
                        }

                    }
                }
                // Convert 1.0 to 255
               col = Color.FromArgb( Convert.ToByte(Red * 255), Convert.ToByte(Green * 255), Convert.ToByte(Blue * 255));

               Console.WriteLine("col = " + col.ToString() );
            }


        // TODO:
        //public void makeRay()
        //{

        //}


        
        private void setColour(int x, int y, Color color)
        {
            drawImage(x, y, color);
        }

        // 
        private void drawImage(int x, int y, Color colour)
        {
            int size = 2;
            byte colR = colour.R;
            byte colG = colour.G;
            byte colB = colour.B;

            DrawRectangle(img, (size + 1) * x, (size + 1) * y, size, size, System.Windows.Media.Color.FromRgb(colR, colG, colB));
        }

        public void DrawRectangle(WriteableBitmap writeableBitmap, int left, int top, int width, int height, System.Windows.Media.Color color)
        {
            // Compute the pixel's color
            int colorData = color.R << 16; // R
            colorData |= color.G << 8; // G
            colorData |= color.B << 0; // B
            int bpp = writeableBitmap.Format.BitsPerPixel / 8;

            unsafe
            {
                for (int y = 0; y < height; y++)
                {
                    // Get a pointer to the back buffer
                    int pBackBuffer = (int)writeableBitmap.BackBuffer;

                    // Find the address of the pixel to draw
                    pBackBuffer += (top + y) * writeableBitmap.BackBufferStride;
                    pBackBuffer += left * bpp;

                    for (int x = 0; x < width; x++)
                    {
                        // Assign the color data to the pixel
                        *((int*)pBackBuffer) = colorData;

                        // Increment the address of the pixel to draw
                        pBackBuffer += bpp;
                    }
                }
            }
        }
    }
}
