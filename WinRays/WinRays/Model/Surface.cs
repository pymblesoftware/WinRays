using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRays.Model
{
    class Surface
    {

        public double Ambient;
        public double Diffuse;
        public double Brilliance;
        public double Specular;
        public double Roughness;
        public double Reflection;
        public Color Colour;

        public Surface(
                            double ambient,
                            double diffuse,
                            double brilliance,
                            double specular,
                            double roughness,
                            double reflection
            )
        {
            Ambient = ambient;
            Diffuse = diffuse;
            Brilliance = brilliance;
            Specular = specular;
            Roughness = roughness;
            Reflection = reflection;
        }
    }
}
