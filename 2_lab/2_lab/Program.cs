using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_lab
{
    struct Vec2f
    {
        public float x { get; private set; }
        public float y { get; private set; }

        public Vec2f(float _x, float _y)
        {
            this.x = _x;
            this.y = _y;
        }
    }

    abstract class Shape
    {
        public Vec2f pos;

        public abstract float GetArea();
        public abstract float GetPerimeter();
        public virtual Vec2f GetCenterOfMass()
        {
            return pos;
        }
    }

    class Circle : Shape
    {
        public float radius { get; private set; }

        public Circle(float _radius, Vec2f _pos)
        {
            radius = _radius;
            this.pos = _pos;
        }

        public override float GetArea()
        {
            return (float)(Math.PI * Math.Pow((radius), 2));
        }

        public override float GetPerimeter()
        {
            return (float)(2 * Math.PI * radius);
        }
    }

    class Ellipse : Shape
    {
        public float a { get; private set; }
        public float b { get; private set; }

        public Ellipse(Vec2f F1, Vec2f F2, float bigO)
        {
            this.Calculate(F1, F2, bigO);
        }

        private void Calculate(Vec2f F1, Vec2f F2, float bigO)
        {
            this.a = bigO / 2;
            var c = Math.Pow((Math.Pow(F1.x - F2.x, 2) + Math.Pow(F1.y - F2.y, 2)), 0.5d);
            var e = c / a;
            this.b = (float)(a * Math.Pow(1 - Math.Pow(e, 2), 0.5d));

            this.pos = new Vec2f((F1.x + F2.x) / 2, (F1.y + F2.y) / 2);
        }

        public override float GetArea()
        {
            return (float)Math.PI * a * b;
        }

        public override float GetPerimeter()
        {
            float p = 4 * (float)((Math.PI * a * b + Math.Pow(a - b, 2)) / (a + b));

            return p;
        }
    }

    class Polygon : Shape
    {
        public List<Vec2f> points = new List<Vec2f>();

        public Polygon(List<Vec2f> _points)
        {
            this.points = _points;
        }

        private void GrahamScan()
        {
            Vec2f[] copy = new Vec2f[points.Count];
            this.points.CopyTo(copy);
            var tmp = new List<Vec2f>(copy);

            int n = tmp.Count;
            var P = new List<int>();
            for (int i=0;i<n;i++)
            {
                P.Add(i);
            }
            for (int i=0;i<n;i++)
            {
                if (tmp[P[i]].x<tmp[P[0]].x)
                {
                    int t = P[i];
                    P[i] = P[0];
                    P[0] = t;
                }
            }

            for (int i=1;i<n;i++)
            {
                int j = i;
                //while (j>1 && )
            }
        }

        public override float GetArea()
        {
            throw new NotImplementedException();
        }

        public override float GetPerimeter()
        {
            throw new NotImplementedException();
        }

        public override Vec2f GetCenterOfMass()
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {


        }
    }
}
