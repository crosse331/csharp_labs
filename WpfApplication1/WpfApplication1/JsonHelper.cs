using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Shapes
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

        public Vec2f GetCopy()
        {
            return new Vec2f(this.x, this.y);
        }

        #region operators
        public static Vec2f operator +(Vec2f v1, Vec2f v2)
        {
            var r = v1.GetCopy();
            r.x += v2.x;
            r.y += v2.y;
            return r;
        }

        public static Vec2f operator -(Vec2f v1, Vec2f v2)
        {
            var r = v2.GetCopy();
            r.x *= -1;
            r.y *= -1;
            r += v1;
            return r;
        }

        public static Vec2f operator /(Vec2f v, float c)
        {
            var r = v.GetCopy();
            r.x /= c;
            r.y /= c;
            return r;
        }
        #endregion

        public override string ToString()
        {
            return string.Format("Vect ({0},{1})", this.x, this.y);
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

        public abstract string InfoToString();

        public override string ToString()
        {
            string result = InfoToString();
            result += "; A: " + this.GetArea().ToString();
            result += "; P: " + this.GetPerimeter().ToString();
            result += "; C: " + this.GetCenterOfMass().ToString();

            return result;
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

        public override string InfoToString()
        {
            return "Circle, r= " + radius.ToString();
        }
    }

    class Ellipse : Shape
    {
        public float a { get; private set; }
        public float b { get; private set; }

        public Vec2f f1;
        public Vec2f f2;

        public Ellipse(Vec2f F1, Vec2f F2, float bigO)
        {
            f1 = F1;
            f2 = F2;
            this.Calculate(F1, F2, bigO);
        }

        private void Calculate(Vec2f F1, Vec2f F2, float bigO)
        {
            this.a = bigO / 2;
            var c = Math.Pow((Math.Pow(F1.x - F2.x, 2) + Math.Pow(F1.y - F2.y, 2)), 0.5d) / 2;
            var e = c / a;
            var tmp = a;
            tmp *= (float)Math.Pow(1 - Math.Pow(e, 2), 0.5d);
            this.b = tmp;

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

        public override string InfoToString()
        {
            return string.Format("Ellipse, f1={0}, f2={1}, a={2}", f1, f2, a);
        }
    }

    class Polygon : Shape
    {
        public List<Vec2f> points = new List<Vec2f>();

        public Polygon(List<Vec2f> _points)
        {
            this.points = _points;

            this.GrahamScan();
        }

        #region MathHelper
        private void GrahamScan()
        {
            Vec2f[] copy = new Vec2f[points.Count];
            this.points.CopyTo(copy);
            var tmp = new List<Vec2f>(copy);

            int n = tmp.Count;
            var P = new List<int>();
            for (int i = 0; i < n; i++)
            {
                P.Add(i);
            }
            for (int i = 0; i < n; i++)
            {
                if (tmp[P[i]].x < tmp[P[0]].x)
                {
                    int t = P[i];
                    P[i] = P[0];
                    P[0] = t;
                }
            }

            for (int i = 1; i < n; i++)
            {
                int j = i;
                while (j > 1 && this.Rotate(tmp[P[0]], tmp[P[j - 1]], tmp[P[j]]) < 0)
                {
                    var t = P[j];
                    P[j] = P[j - 1];
                    P[j - 1] = P[j];
                    j--;
                }
            }

            this.points = tmp;
        }

        private float Rotate(Vec2f p1, Vec2f p2, Vec2f p3)
        {
            return (p2.x - p1.x) * (p3.y - p2.y) - (p2.y - p1.x) * (p3.x - p2.y);
        }

        private float GetDistance(Vec2f p1, Vec2f p2)
        {
            return (float)Math.Pow((Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2)), 0.5d);
        }

        private float GetTriangleArea(Vec2f p1, Vec2f p2, Vec2f p3)
        {
            float a = GetDistance(p1, p2);
            float b = GetDistance(p2, p3);
            float c = GetDistance(p3, p1);

            float p = (a + b + c) / 2;

            return (float)Math.Pow(p * (p - a) * (p - b) * (p - c), 0.5d);
        }

        #endregion
        public override float GetArea()
        {
            float S = 0;
            for (int i = 1; i < this.points.Count - 1; i++)
            {
                S += this.GetTriangleArea(this.points[0], this.points[i], this.points[i + 1]);
            }

            return S;
        }

        public override float GetPerimeter()
        {
            float P = 0;
            for (int i = 1; i < this.points.Count; i++)
            {
                P += this.GetDistance(this.points[i - 1], this.points[i]);
            }
            P += this.GetDistance(this.points[0], this.points[this.points.Count - 1]);

            return P;
        }

        public override Vec2f GetCenterOfMass()
        {
            var result = new Vec2f(0, 0);

            for (int i = 0; i < this.points.Count; i++)
            {
                result += this.points[i];
            }

            result /= this.GetPerimeter();

            return result;
        }

        public override string InfoToString()
        {
            string result = "Polygon, dots: ";
            for (int i = 0; i < this.points.Count; i++)
            {
                result += this.points[i].ToString() + " ";
            }

            return result;
        }
    }

    [DataContractAttribute]
    abstract class ShapeInfo
    {
        [DataMemberAttribute]
        public float[] pos = new float[2];
    }

    [DataContractAttribute]
    class CircleInfo : ShapeInfo
    {
        [DataMemberAttribute]
        public float radius;
        public CircleInfo(Circle c)
        {
            this.pos[0] = c.pos.x;
            this.pos[1] = c.pos.y;
            this.radius = c.radius;
        }
    }

    [DataContractAttribute]
    class EllipseInfo : ShapeInfo
    {
        [DataMemberAttribute]
        public float[] f1 = new float[2];
        [DataMemberAttribute]
        public float[] f2 = new float[2];
        [DataMemberAttribute]
        public float bigO;

        public EllipseInfo(Ellipse e)
        {
            this.f1[0] = e.f1.x;
            this.f1[1] = e.f1.y;
            this.f2[0] = e.f2.x;
            this.f2[1] = e.f2.y;

            this.bigO = e.a * 2;
        }
    }

    [DataContractAttribute]
    class PolygonInfo : ShapeInfo
    {
        [DataMemberAttribute]
        public List<float[]> points = new List<float[]>();

        public PolygonInfo(Polygon p)
        {
            //this.points = new float[p.points.Count, 2];
            for (int i = 0; i < p.points.Count; i++)
            {
                this.points.Add(new float[2] { p.points[i].x, p.points[i].y });
            }
        }
    }

}
