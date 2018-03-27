using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_lab
{
    class Program
    {
        struct Ratio
        {
            public int top;
            public int bottom;

            public Ratio( int _t, int _b)
            {
                this.top = _t;
                this.bottom = _b;
            }

            public Ratio GetCopy()
            {
                var result = new Ratio(this.top, this.bottom);
                return result;
            }

            private void Simple()
            {
                if (this.bottom == 0)
                {
                    throw new ArgumentOutOfRangeException("The bottom is 0!");
                }
                int d = GCD(this.bottom, this.top);

                this.bottom /= d;
                this.top /= d;
            }

            static int GCD(int a, int b)
            {
                return b == 0 ? (a < 0 ? -a : a) : GCD(b, a % b);
            }

            #region operators

            public static Ratio operator +(Ratio r1, Ratio r2)
            {
                var nRatio = r1.GetCopy();
                var tmpRatio = r2.GetCopy();
                nRatio.top *= r2.bottom;
                nRatio.bottom *= r2.bottom;
                tmpRatio.top *= r1.bottom;
                tmpRatio.bottom *= r1.bottom;

                nRatio.top += tmpRatio.top;

                nRatio.Simple();

                return nRatio;
            }

            public static Ratio operator -(Ratio r1, Ratio r2)
            {
                r2.top *= -1;

                return r1 + r2;
            }

            public static Ratio operator -(Ratio r1)
            {
                r1.top *= -1;

                return r1;
            }

            public static Ratio operator +(Ratio r1)
            {
                return r1;
            }

            public static Ratio operator *(Ratio r1, Ratio r2)
            {
                var r = r1.GetCopy();
                r.top *= r2.top;
                r.bottom *= r2.bottom;

                r.Simple();

                return r;
            }

            public static Ratio operator /(Ratio r1, Ratio r2)
            {
                var r = r1.GetCopy();
                var tmp = new Ratio(r2.bottom, r2.top);

                return r * tmp;
            }
            #endregion

            public double ToDouble()
            {
                return ((double)this.top) / this.bottom;
            }

            public override string ToString()
            {
                return string.Format("{0}/{1}", this.top, this.bottom);
            }
        }


        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please, print two ratio numbers in format: N1t N1b N2t N2b (t is top, b is bottom) ");
                string input = Console.ReadLine();

                if (input == "q" || input == "Q")
                {
                    break;
                }

                var mass = input.Split(' ');
                List<int> list = new List<int>();
                for (int i=0;i<mass.Length;i++)
                {
                    if (mass[i]!="")
                    {
                        list.Add(Convert.ToInt32(mass[i]));
                        if (list.Count>4)
                        {
                            throw new Exception("Bad input numbers lenght!");
                        }
                    }
                }

                var rat1 = new Ratio(list[0], list[1]);
                var rat2 = new Ratio(list[2], list[3]);

                Console.WriteLine("N1 + N2 = " + (rat1 + rat2).ToString());
                Console.WriteLine("N1 - N2 = " + (rat1 - rat2).ToString());
                Console.WriteLine("-N1 = " + (-rat1).ToString());
                Console.WriteLine("+N1 = " + (+rat1).ToString());
                Console.WriteLine("N1 * N2 = " + (rat1 * rat2).ToString());
                Console.WriteLine("N1 / N2 = " + (rat1 / rat2).ToString());
                Console.WriteLine("N1 to double =" + (rat1.ToDouble()).ToString());
            }
        }
    }
}
