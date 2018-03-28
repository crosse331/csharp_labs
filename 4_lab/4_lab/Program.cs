using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4_lab
{
    class Vector<T>
    {
        List<T> elements = new List<T>();

        public T GetElement(int index)
        {
            return elements[index];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
