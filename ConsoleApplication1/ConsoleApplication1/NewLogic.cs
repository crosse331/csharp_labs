using System;
using System.Collections;
using System.Collections.Generic;

namespace NewLogic
{
    public static class NewLogic
    {
        static Random randomizer = new Random();
        public static T GetRandomElement<T>(List<T> list)
        {

            return list[randomizer.Next(0, list.Count)];
        }

        public static T GetRandomElement<T>(List<T> list, List<float> chances)
        {
            while (list.Count > chances.Count)
            {
                list.RemoveAt(list.Count - 1);
            }
            while (list.Count < chances.Count)
            {
                chances.RemoveAt(chances.Count - 1);
            }

            float total = 0f;
            for (int i = 0; i < chances.Count; i++)
            {
                total += chances[i];
            }

            float chance = (float)randomizer.NextDouble() * total;

            total = 0f;
            for (int i = 0; i < list.Count; i++)
            {
                total += chances[i];
                if (total > chance)
                {
                    return list[i];
                }
            }

            return list[0];
        }
    }

    public class Stat
    {
        private float max;
        private float cur;

        public Stat(float value)
        {
            this.max = value;
            this.cur = value;
        }

        #region operators

        public static Stat operator +(Stat s1, float val)
        {
            s1.cur += val;
            if (s1.cur > s1.max)
            {
                s1.cur = s1.max;
            }
            return s1;
        }

        public static Stat operator -(Stat s1, float val)
        {
            //Debug.Log(val);
            s1.cur -= val;
            if (s1.cur < 0)
            {
                s1.cur = 0;
            }
            return s1;
        }

        public static bool operator ==(Stat s1, float val)
        {
            return s1.cur == val;
        }

        public static bool operator !=(Stat s1, float val)
        {
            return s1.cur != val;
        }

        public static bool operator <(Stat s1, float val)
        {
            return s1.cur < val;
        }

        public static bool operator >(Stat s1, float val)
        {
            return s1.cur > val;
        }

        public static bool operator <=(Stat s1, float val)
        {
            return s1.cur <= val;
        }

        public static bool operator >=(Stat s1, float val)
        {
            return s1.cur >= val;
        }

        #endregion

        public override string ToString()
        {
            return ((int)this.cur).ToString() + "/" + ((int)this.max).ToString();
        }

        public float GetCurrent()
        {
            return cur;
        }

        public float GetCurrentCoef()
        {
            return cur / max;
        }

        public float GetMax()
        {
            return max;
        }
    }
}