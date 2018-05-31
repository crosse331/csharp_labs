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

        public static World mainWorld { get; private set; }

        static void Main(string[] args)
        {
            MainConsole = new RLRootConsole("terminal8x8.png", 50, 50, 8, 8, 2, "Coop roguelike");

            MainConsole.Update += OnRootConsoleUpdate;
            MainConsole.Render += OnRootConsoleRender;

            Init();

            MainConsole.Run();
        }

        private static void Init()
        {
            var player = new Player((char)2, Vector.One * 10, new RLColor(0.4f, 0.4f, 0.4f));
            mainWorld = new World();
        }

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            CreaturesContainer.MovingLogic();
        }

        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            MainConsole.Clear();
            //MainConsole.Print(30, 20, "@", RLColor.White);
            mainWorld.Render(MainConsole);
            CreaturesContainer.RenderLogic(MainConsole);
            MainConsole.Draw();
        }
    }

    public class World
    {
        private const int WALL = 219;

        private int[,] map = new int[50, 50];
        private Random randomizer = new Random();

        public World()
        {
            this.Generate();
        }

        public void Generate()
        {
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    if (i == 0 || j == 0 || i == 49 || j == 49)
                    {
                        map[i, j] = WALL;
                    }
                    else
                    {
                        if (randomizer.Next(0, 100) > 85)
                        {
                            map[i, j] = WALL;
                        }
                    }
                }
            }
        }

        public void Render(RLConsole console)
        {
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    console.Print(i, j, ((char)(map[i, j] > 0 ? map[i, j] : 0)).ToString(), RLColor.White);
                }
            }
        }

        public bool CheckPosition(Vector pos)
        {
            if (map[pos.X, pos.Y] != 0)
            {
                return false;
            }

            return true;
        }

        public void Move(Vector from, Vector to)
        {
            map[from.X, from.Y] = 0;
            map[to.X, to.Y] = -1;
        }
    }
}
