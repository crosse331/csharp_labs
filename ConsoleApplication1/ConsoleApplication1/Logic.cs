using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public static class Logic
    {

    }

    public struct Vector
    {
        public static readonly Vector One = new Vector(1, 1);
        public static readonly Vector Zero = new Vector(0, 0);
        public static readonly Vector Right = new Vector(1, 0);
        public static readonly Vector Up = new Vector(0, -1);

        public int X;
        public int Y;

        public Vector(int _x, int _y)
        {
            this.X = _x;
            this.Y = _y;
        }

        public Vector(Vector v)
        {
            this.X = v.X;
            this.Y = v.Y;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            var tmp = new Vector(v1);
            tmp.X += v2.X;
            tmp.Y += v2.Y;
            return tmp;
        }

        public static Vector operator *(Vector v1, int k)
        {
            v1.X *= k;
            v1.Y *= k;
            return v1;
        }

        public static Vector operator -(Vector v1)
        {
            return v1 * -1;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return v1 + (-v2);
        }
    }
}
