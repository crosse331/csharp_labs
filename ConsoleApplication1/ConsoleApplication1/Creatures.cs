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

        protected Vector prevPos;

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

        public virtual void TryToMove(Vector dir)
        {
            prevPos = this.position;
            var world = Program.mainWorld;
            if (world.CheckPosition(this.position + dir))
            {
                world.Move(this.position, this.position + dir);
                this.position = this.position + dir;
            }
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
                    case RLKey.Up: this.TryToMove(Vector.Up); break;
                    case RLKey.Down: this.TryToMove(-Vector.Up); break;
                    case RLKey.Left: this.TryToMove(-Vector.Right); break;
                    case RLKey.Right: this.TryToMove(Vector.Right); break;
                }
            }
        }
    }

    public class TestEnemy : Creature
    {
        public TestEnemy(char s, Vector pos) : base(s, pos) { }
        public TestEnemy(char s, Vector pos, RLColor c) : base(s, pos, c) { }

        private Random randomizer = new Random();

        public override void MovingLogic()
        {
            base.MovingLogic();

            int rnd = randomizer.Next(0, 100);
            if (rnd < 25)
            {
                this.TryToMove(-Vector.Right);
            }
            else if (rnd < 50)
            {
                this.TryToMove(Vector.Up);
            }
            else if (rnd < 75)
            {
                this.TryToMove(Vector.Right);
            }
            else if (rnd < 100)
            {
                this.TryToMove(-Vector.Up);
            }
        }

        public override void TryToMove(Vector dir)
        {
            base.TryToMove(dir);

            if (this.prevPos == this.position)
            {
                var crea = CreaturesContainer.GetCreatureOnPosition(this.position + dir);
                if (crea != null)
                {

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
            for (int i = 0; i < allCreatures.Count; i++)
            {
                allCreatures[i].MovingLogic();
            }
        }

        public static void RenderLogic(RLConsole console)
        {
            for (int i = 0; i < allCreatures.Count; i++)
            {
                allCreatures[i].Render(console);
            }
        }

        public static Creature GetCreatureOnPosition(Vector pos)
        {
            Creature result = null;
            for (int i = 0; i < allCreatures.Count; i++)
            {
                if (allCreatures[i].position == pos)
                {
                    result = allCreatures[i];
                }
            }

            return result;
        }
    }
}
