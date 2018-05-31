using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RLNET;

namespace ConsoleApplication1
{
    public class Creature
    {
        public Vector position { get; protected set; }
        public char symbol { get; protected set; }
        public RLColor color { get; protected set; }

        public Creature(char s, Vector pos)
        {
            this.symbol = s;
            this.position = pos;
            this.color = RLColor.White;

            CreaturesContainer.AddCreature(this);
        }

        public Creature(char s, Vector pos, RLColor c)
        {
            this.symbol = s;
            this.position = pos;
            this.color = c;

            CreaturesContainer.AddCreature(this);
        }

        public void TryToMove(Vector dir)
        {
            //iteract with map
        }

        public virtual void MovingLogic()
        {

        }

        public virtual void Render(RLConsole console)
        {
            console.Print(this.position.X, this.position.Y, this.symbol.ToString(), this.color);
        }
    }

    public class Player : Creature
    {
        public Player(char s, Vector pos) : base(s, pos) { }
        public Player(char s, Vector pos, RLColor c) : base(s, pos, c) { }

        public override void MovingLogic()
        {
            base.MovingLogic();

            var keyPress = Program.MainConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                switch (keyPress.Key)
                {
                    case RLKey.Up: this.position += Vector.Up; break;
                    case RLKey.Down: this.position -= Vector.Up; break;
                    case RLKey.Left: this.position -= Vector.Right; break;
                    case RLKey.Right: this.position += Vector.Right; break;
                }
            }
        }
    }

    public static class CreaturesContainer
    {
        public static List<Creature> allCreatures = new List<Creature>();

        public static void AddCreature(Creature c)
        {
            allCreatures.Add(c);
        }

        public static void MovingLogic()
        {
            for (int i=0;i<allCreatures.Count;i++)
            {
                allCreatures[i].MovingLogic();
            }
        }

        public static void RenderLogic(RLConsole console)
        {
            for (int i=0;i<allCreatures.Count;i++)
            {
                allCreatures[i].Render(console);
            }
        }
    }
}
