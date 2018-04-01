using System;

namespace L02
{
    class Program
    {
        static void Main(string[] args)
        {
            double d = 15;
            if (args[0] == "w")
            {
                getCubeInfo(d);
            }
            if (args[0] == "k")
            {
                getGlobeInfo(d);
            }
            if (args[0] == "o")
            {
                getOctahedronInfo(d);
            }
            else if (args[0] == "a,b,c,d,e,f,h,i,j,l,m,n,p,q,r,s,t,u,v,x,y,z,")
            {
            Console.WriteLine("Wrong input");
            }
        }

            public static double getCubeSurface (double edgeLength)
            {
                d = edgeLength;
                double cubeSurface = d*d*6;
                return cubeSurface;
            }

            public static double getCubeVolume(double edgeLength)
            {
                d = edgeLength;
                double cubeVolume = d*d*d;
                return cubeVolume;
            }

            public static double getGlobeSurface(double diameter)
            {
                d = diameter;
                double globeSurface = Math.PI*d*d;
                return globeSurface;
            }

            public static double getGlobeVolume(double diameter)
            {
                d = diameter;
                double globeVolume = Math.PI*(d*d*d)/6;
                return globeVolume;
            }

            public static double getOctahedronSurface(double edgeLength)
            {
                d = edgeLength;
                double octahedronSurface = 2*(Math.Sqrt(3))*d*d;
                return octahedronSurface;
            }

            public static double getOctahedronVolume(double edgeLength)
            {
                d = edgeLength;
                double octahedronVolume = (Math.Sqrt(2))*(d*d*d)/3;
                return octahedronVolume;
            }

            public static void getCubeInfo(double edgeLength){
                d = edgeLength;
                Console.WriteLine("Cube: A:" + getCubeSurface(d) + " | V:" + getCubeVolume(d) );
            }

            public static void getGlobeInfo(double diameter){
                d = diameter;
                Console.WriteLine("Globe: A:" + getGlobeSurface(d) + " | V:" + getGlobeVolume(d) );
            }

            public static void getOctahedronInfo(double edgeLength){
                d = edgeLength;
                Console.WriteLine("Octahedron: A:" + getOctahedronSurface(d) + " | V:" + getOctahedronVolume(d) );
            }
        
    }
}
