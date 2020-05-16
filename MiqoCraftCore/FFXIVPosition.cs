using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiqoCraftCore
{
    public class FFXIVPosition
    {
        public double X = 0;
        public double Y = 0;
        public double Z = 0;

        public FFXIVPosition()
        {
        }

        public FFXIVPosition(double iX, double iY, double iZ = 0)
        {
            X = iX;
            Y = iY;
            Z = iZ;
        }

        public double DistanceTo(FFXIVPosition iOtherPosition)
        {
            if (null == iOtherPosition) return 9999;

            return Math.Sqrt(Math.Pow(iOtherPosition.X - X, 2) + Math.Pow(iOtherPosition.Y - Y, 2) + Math.Pow(iOtherPosition.Z - Z, 2));
        }

        public double PlanarDistanceTo(FFXIVPosition iOtherPosition)
        {
            if (null == iOtherPosition) return 9999;

            return Math.Sqrt(Math.Pow(iOtherPosition.X - X, 2) + Math.Pow(iOtherPosition.Y - Y, 2));
        }

        public override string ToString()
        {
            return "[" + X + "; " + Y + "; " + Z + "]";
        }
    }
}
