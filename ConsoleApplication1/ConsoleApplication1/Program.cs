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
        private static Rect mainConsoleSize = new Rect(0, 0, 60, 40);

        public static RLConsole statsConsole { get; private set; }
        private static Rect statsConsoleRect = new Rect(0, 0, 20, 5);
        public static RLConsole worldConsole { get; private set; }
        private static Rect worldConsoleRect = new Rect(0, 5, 55, 35);

        private static Hud mainHud;

        public static World mainWorld { get; private set; }

        static void Main(string[] args)
        {
            MainConsole = new RLRootConsole("terminal8x8.png", 
                mainConsoleSize.Size.X, mainConsoleSize.Size.Y, 8, 8, 1, "Coop roguelike");

            statsConsole = new RLConsole(statsConsoleRect.Size.X, statsConsoleRect.Size.Y);
            worldConsole = new RLConsole(worldConsoleRect.Size.X, worldConsoleRect.Size.Y);

            MainConsole.Update += OnRootConsoleUpdate;
            MainConsole.Render += OnRootConsoleRender;

            Init();

            MainConsole.Run();
        }

        private static void Init()
        {
            var player = new Player((char)2, Vector.One * 10, RLColor.White);/*new RLColor(0.4f, 0.4f, 0.4f));*/
            var enemy = new TestEnemy((char)1, Vector.One * 15, RLColor.White);

            mainHud = new Hud(player);

            mainWorld = new World();
        }

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            CreaturesContainer.MovingLogic();
            TimersContainer.Logic();

        }

        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            MainConsole.Clear();
            //MainConsole.Print(30, 20, "@", RLColor.White);
            
            RLConsole.Blit(statsConsole, 0, 0, statsConsoleRect.Size.X, statsConsoleRect.Size.Y,
                MainConsole, statsConsoleRect.Pos.X, statsConsoleRect.Pos.Y);
            RLConsole.Blit(worldConsole, 0, 0, worldConsoleRect.Size.X, worldConsoleRect.Size.Y,
                MainConsole, worldConsoleRect.Pos.X, worldConsoleRect.Pos.Y);

            mainWorld.Render(worldConsole);
            CreaturesContainer.RenderLogic(worldConsole);
            mainHud.Render(statsConsole);

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

    public class Hud
    {
        public Creature target;
        public StatsPanel heroPanel;

        public Hud(Creature tar)
        {
            this.target = tar;
            heroPanel = new StatsPanel(tar);
        }

        public void Render(RLConsole console)
        {
            heroPanel.Render(console);
        }

    }
}
