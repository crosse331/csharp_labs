using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RLNET;

namespace ConsoleApplication1
{
    class Program
    {
        public static RLRootConsole MainConsole { get; private set; }

        static void Main(string[] args)
        {
            MainConsole = new RLRootConsole("terminal8x8.png", 60, 40, 8, 8);

            MainConsole.Update += OnRootConsoleUpdate;
            MainConsole.Render += OnRootConsoleRender;

            Init();

            MainConsole.Run();
        }

        private static void Init()
        {
            var player = new Player((char)2, Vector.One * 10, new RLColor(0.4f, 0.4f, 0.4f));
        }

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            CreaturesContainer.MovingLogic();
        }

        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            MainConsole.Clear();
            //MainConsole.Print(30, 20, "@", RLColor.White);
            CreaturesContainer.RenderLogic(MainConsole);
            MainConsole.Draw();
        }
    }

    class World
    {
        private int[,] render = new int[10, 10];


    }
}
