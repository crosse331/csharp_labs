using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathLogic;
using RLNET;

namespace ConsoleApplication1
{
    public enum HorzAnchor
    {
        Center,
        Left,
        Right
    }

    public enum VertAnchor
    {
        Center,
        Top,
        Bottom
    }

    class Gui
    {
    }

    public class StatsPanel
    {
        private const int PROGRESS_BAR = 220;

        public Creature target;

        private int hpCount = 0;

        public StatsPanel(Creature target)
        {
            this.target = target;
        }

        public void Render(RLConsole console)
        {
            int hpLenght = (int)target.Hp.GetMax() / 50;
            int enLenght = (int)target.Stamina.GetMax() / 50;

            hpCount = (int)(Math.Round((double)target.Hp.GetCurrentCoef() * hpLenght));
            int enCount = (int)(Math.Round((double)target.Stamina.GetCurrentCoef() * enLenght));

            string hpStr = "";
            for (int i = 0; i < hpLenght; i++)
            {
                hpStr += i < hpCount ? ((char)PROGRESS_BAR).ToString() : " ";
                //hpStr += ((char)PROGRESS_BAR).ToString();
            }
            string enStr = "";
            for (int i = 0; i < enLenght; i++)
            {
                enStr += i < enCount ? ((char)PROGRESS_BAR).ToString() : " ";
            }
            console.Print(1, 1, hpStr, RLColor.Red);
            console.Print(1, 2, enStr, RLColor.Green);
        }
    }

    public class Hud
    {
        public Creature target;
        public StatsPanel heroPanel;

        public Hud(Creature tar)
        {
            if (tar != null)
            {
                this.target = tar;
                heroPanel = new StatsPanel(tar);
            }
        }

        public void Render(RLConsole console)
        {
            if (heroPanel != null)
            {
                heroPanel.Render(console);
            }
        }

    }

    public class StrListItem
    {
        public string title;
        public Action onEnter;

        public StrListItem(string ttl, Action onntr)
        {
            this.title = ttl;
            this.onEnter = onntr;
        }

        public void Activate()
        {
            if (onEnter != null)
            {
                onEnter();
            }
        }

        public override string ToString()
        {
            return title;
        }
    }

    public class ObjectList
    {
        public List<StrListItem> items = new List<StrListItem>();
        //public Dictionary<>
        private int _curIndex = 0;

        private int _lineSpacing = 1;
        //private HorzAnchor _horzAnchor = HorzAnchor.Center;
        //private VertAnchor _vertAnchor = VertAnchor.Center;

        public ObjectList(List<StrListItem> _itms)
        {
            for (int i = 0; i < _itms.Count; i++)
            {
                this.items.Add(_itms[i]);
            }
        }

        public void Render(RLConsole console)
        {
            int height = console.Height;
            int width = console.Width;
            int listSize = items.Count + (_lineSpacing - 1) * items.Count;

            Vector startPos = Vector.Zero;

            for (int i = 0; i < items.Count; i++)
            {
                string tmp = items[i].ToString();
                startPos.X = (width - tmp.Length) / 2;
                startPos.Y = (height - items.Count - _lineSpacing * (items.Count - 1)) / 2 + i * (1 + _lineSpacing);

                if (i == _curIndex)
                {
                    console.Print(startPos.X, startPos.Y, items[i].ToString(), RLColor.Black, RLColor.White);
                }
                else
                {
                    console.Print(startPos.X, startPos.Y, items[i].ToString(), RLColor.White);
                }
            }
        }

        public void Logic()
        {
            var keyPress = Program.MainConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                switch (keyPress.Key)
                {
                    case RLKey.Up:
                        this._curIndex--;
                        if (this._curIndex < 0)
                        {
                            this._curIndex = 0;
                        }
                        break;
                    case RLKey.Down:
                        this._curIndex++;
                        if (this._curIndex > items.Count - 1)
                        {
                            this._curIndex = items.Count - 1;
                        }
                        break;

                    case RLKey.Enter:
                    case RLKey.KeypadEnter:
                        this.items[_curIndex].Activate();
                        break;
                }

            }
        }
    }

    public abstract class GameState
    {
        public abstract void Render(RLConsole mainConsole);
        public abstract void Logic();
        public abstract void Init();
    }

    public class MainGame : GameState
    {
        public RLConsole statsConsole { get; private set; }
        protected Rect statsConsoleRect = new Rect(0, 0, 20, 5);
        public RLConsole worldConsole { get; private set; }
        protected Rect worldConsoleRect = new Rect(0, 5, 55, 35);

        protected Hud mainHud;

        public World mainWorld { get; private set; }

        private Server testServer;

        private Player curPlayer;

        public override void Render(RLConsole mainConsole)
        {
            RLConsole.Blit(statsConsole, 0, 0, statsConsoleRect.Size.X, statsConsoleRect.Size.Y,
                mainConsole, statsConsoleRect.Pos.X, statsConsoleRect.Pos.Y);
            RLConsole.Blit(worldConsole, 0, 0, worldConsoleRect.Size.X, worldConsoleRect.Size.Y,
                mainConsole, worldConsoleRect.Pos.X, worldConsoleRect.Pos.Y);

            mainWorld.Render(worldConsole);
            CreaturesContainer.RenderLogic(worldConsole);
            AttacksContainer.Render(worldConsole);
            mainHud.Render(statsConsole);
        }

        public override void Logic()
        {
            CreaturesContainer.MovingLogic();
            AttacksContainer.Logic();
            NetContainer.Logic();
            testServer.Logic();
            mainWorld.Logic(curPlayer);
        }

        public override void Init()
        {
            statsConsole = new RLConsole(statsConsoleRect.Size.X, statsConsoleRect.Size.Y);
            worldConsole = new RLConsole(worldConsoleRect.Size.X, worldConsoleRect.Size.Y);

            curPlayer = new Player((char)2, Vector.One * 10, RLColor.White);/*new RLColor(0.4f, 0.4f, 0.4f));*/
            //var enemy = new TestEnemy((char)1, Vector.One * 15, RLColor.White);

            mainHud = new Hud(curPlayer);

            mainWorld = new World();

            testServer = new Server();
            testServer.Init();
        }
    }

    public class ClientMainGame : GameState
    {
        public RLConsole statsConsole { get; private set; }
        protected Rect statsConsoleRect = new Rect(0, 0, 20, 5);
        public RLConsole worldConsole { get; private set; }
        protected Rect worldConsoleRect = new Rect(0, 5, 55, 35);

        protected Hud mainHud;

        public World mainWorld { get; private set; }

        public Client client;

        private Player curPlayer;

        public override void Render(RLConsole mainConsole)
        {
            RLConsole.Blit(statsConsole, 0, 0, statsConsoleRect.Size.X, statsConsoleRect.Size.Y,
                mainConsole, statsConsoleRect.Pos.X, statsConsoleRect.Pos.Y);
            RLConsole.Blit(worldConsole, 0, 0, worldConsoleRect.Size.X, worldConsoleRect.Size.Y,
                mainConsole, worldConsoleRect.Pos.X, worldConsoleRect.Pos.Y);

            mainWorld.Render(worldConsole);
            CreaturesContainer.RenderLogic(worldConsole);
            AttacksContainer.Render(worldConsole);
            mainHud.Render(statsConsole);
        }

        public override void Logic()
        {
            CreaturesContainer.MovingLogic();
            AttacksContainer.Logic();
            NetContainer.Logic();
            mainWorld.Logic(curPlayer);
        }

        public override void Init()
        {
            statsConsole = new RLConsole(statsConsoleRect.Size.X, statsConsoleRect.Size.Y);
            worldConsole = new RLConsole(worldConsoleRect.Size.X, worldConsoleRect.Size.Y);

            //var player = new Player((char)2, Vector.One * 10, RLColor.White);/*new RLColor(0.4f, 0.4f, 0.4f));*/
            //var enemy = new TestEnemy((char)1, Vector.One * 15, RLColor.White);

            mainHud = new Hud(null);

            mainWorld = new World();

            client = new Client();
            client.Init();

            Program.isServer = false;
        }
    }

    public class MainMenu : GameState
    {
        public RLConsole menuConsole { get; private set; }

        private ObjectList mainMenuList = new ObjectList(new List<StrListItem>()
        {
            new StrListItem("New Game", () => { Program.currentState = new MainGame(); }),
            new StrListItem("Connect", () => {Program.currentState = new ClientMainGame(); }),
            new StrListItem("Quit", () => { Program.MainConsole.Close(); })
        });

        public override void Init()
        {
            //throw new NotImplementedException();

            menuConsole = new RLConsole(Program.MainConsole.Width, Program.MainConsole.Height);
        }

        public override void Logic()
        {
            mainMenuList.Logic();
        }

        public override void Render(RLConsole console)
        {
            RLConsole.Blit(menuConsole, 0, 0, console.Width, console.Height, console,
                0, 0);

            mainMenuList.Render(console);
        }
    }

    public class NetTester : GameState
    {
        Client client;

        public override void Init()
        {
            client = new Client();
            client.Init();
        }

        public override void Logic()
        {
            client.Logic();
        }

        public override void Render(RLConsole mainConsole)
        {
            
        }
    }
}
