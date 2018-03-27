using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_lab
{
    class Program
    {
        static List<int> numbers = new List<int>();

        static Random randomizer = new Random();

        static void Main(string[] args)
        {
            int count = randomizer.Next(10, 30);
            for (int i=0;i<count;i++)
            {
                numbers.Add(randomizer.Next(-100, 100));
            }

            Console.WriteLine("Default:");
            foreach(var num in numbers)
            {
                Console.Write(string.Format("{0} ", num));
            }
            Console.WriteLine();
            numbers.Sort((x, y) =>
            {
                return x.CompareTo(y);
            });
            Console.WriteLine("Sorted:");
            foreach (var num in numbers)
            {
                Console.Write(string.Format("{0} ", num));
            }
        }
    }
}
