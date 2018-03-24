using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _0_lab
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Wrong args count!");
                return;
            }

            string firstFileName = args[0];
            string secondFileName = args[1];

            StreamReader firstReader = new StreamReader(firstFileName);
            StreamReader secondReader = new StreamReader(secondFileName);

            while (!firstReader.EndOfStream && !secondReader.EndOfStream)
            {
                int s1 = firstReader.Read();
                int s2 = secondReader.Read();

                if (s1 != s2)
                {
                    PrintDiff(firstReader, firstFileName);
                    PrintDiff(secondReader, secondFileName);
                    break;
                }
            }
        }

        static void PrintDiff(StreamReader reader, string name)
        {
            Console.WriteLine(string.Format("File {0}:", name));

            for (int i = 0; i < 16; i++)
            {
                if (!reader.EndOfStream)
                {
                    Console.WriteLine(reader.Read());
                }
                else
                {
                    Console.WriteLine("End of File!");
                    break;
                }
            }
        }
    }
}
