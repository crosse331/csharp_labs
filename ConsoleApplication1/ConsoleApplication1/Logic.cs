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

        public static bool operator ==(Vector v1, Vector v2)
        {
            return (v1.X == v2.X) && (v1.Y == v2.Y);
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return !(v1 == v2);
        }
    }

    public struct Rect
    {
        public Vector Pos;
        public Vector Size;

        public Rect(int x, int y, int w, int h)
        {
            this.Pos = new Vector(x, y);
            this.Size = new Vector(w, h);
        }
    }

    public class Timer
    {
        public string name { get; }
        public int ticks = 0;
        public int finalTick = 0;
        public Action onFinish = null;

        public Timer(string n, int t, Action onF)
        {
            this.name = n;
            this.finalTick = t;
            this.onFinish += onF;

            TimersContainer.Add(this);
        }

        public void Logic()
        {
            this.ticks++;
            if (this.ticks >= finalTick)
            {
                if (onFinish!=null)
                {
                    onFinish();
                    onFinish = null;
                }
                TimersContainer.Remove(this);
            }
            
        }
    }

    public class Animation
    {
        public List<char> animChars = new List<char>();
        private int time = 0;
        private Creature owner = null;

        private Timer timer;

        private Action onFinishAction = null;

        private RLNET.RLColor _startColor;
        private char _startChar;

        public Animation(List<char> list, int tm, Creature ownr, Action onFinish)
        {
            for (int i=0;i<list.Count;i++)
            {
                this.animChars.Add(list[i]);
            }
            this.time = tm;
            this.owner = ownr;

            this.onFinishAction = onFinish;

            this._startColor = owner.color;
            this._startChar = owner.symbol;

            this.timer = new Timer(owner.symbol + "'s animation", this.time, OnTimeEnded);
        }

        public void Render()
        {
            if (animChars.Count == 0)
            {

            }
            else if (animChars.Count == 1)
            {
                this.owner.symbol = this.animChars[0];
                float percent = (float)this.timer.ticks / this.timer.finalTick;
                this.owner.color = RLNET.RLColor.White * percent;
            }
        }

        public void OnTimeEnded()
        {
            this.owner.color = this._startColor;
            this.owner.symbol = this._startChar;
            if (onFinishAction!=null)
            {
                this.onFinishAction();
            }
        }
    }
}
