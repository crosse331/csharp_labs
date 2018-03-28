using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Shapes
{
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
