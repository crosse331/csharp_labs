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
        public static bool isServer = true;

        public static RLRootConsole MainConsole { get; private set; }
        private static Rect mainConsoleSize = new Rect(0, 0, 60, 40);

        public static RLConsole mainMenuConsole { get; private set; }

        private static GameState _curState;
        public static GameState currentState
        {
            get
            {
                return _curState;
            }
            set
            {
                if (value != _curState)
                {
                    _curState = value;
                    _curState.Init();
                }
            }
        }

        static void Main(string[] args)
        {
            MainConsole = new RLRootConsole("terminal8x8.png",
                mainConsoleSize.Size.X, mainConsoleSize.Size.Y, 8, 8, 1, "Roguelike");

            currentState = new MainMenu();

            Init();

            MainConsole.Update += OnRootConsoleUpdate;
            MainConsole.Render += OnRootConsoleRender;
            
            MainConsole.Run();
        }

        private static void Init()
        {
        }

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            TimersContainer.Logic();

            if (currentState!=null)
            {
                currentState.Logic();
            }
        }

        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            MainConsole.Clear();
            //MainConsole.Print(30, 20, "@", RLColor.White);

            if (currentState!=null)
            {
                currentState.Render(MainConsole);
            }

            MainConsole.Draw();
        }

        public static World GetWorld()
        {
            if (!(currentState is MainGame))
            {
                return null;
            }
            else
            {
                return (currentState as MainGame).mainWorld;
            }
        }
    }
}
