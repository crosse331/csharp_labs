using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RLNET;
using MathLogic;

namespace ConsoleApplication1
{
    public class Creature
    {
        public Vector position { get; protected set; }
        public Vector facing { get; protected set; }
        public char symbol { get; protected set; }
        public RLColor color { get; protected set; }

        public int movingDelay = 10;
        private bool canMove = true;
        private bool canAttack = true;

        public Stat Hp = new Stat(350);
        public Stat Energy = new Stat(300);

        private bool isRestoringStamina = true;

        private List<Timer> timers = new List<Timer>();

        protected Vector prevPos;

        public Creature(char s, Vector pos)
        {
            this.symbol = s;
            this.position = pos;
            this.color = RLColor.White;

            CreaturesContainer.Add(this);
        }

        public Creature(char s, Vector pos, RLColor c)
        {
            this.symbol = s;
            this.position = pos;
            this.color = c;

            CreaturesContainer.Add(this);
        }

        public virtual void TryToMove(Vector dir)
        {
            if (!this.canMove)
            {
                return;
            }
            prevPos = this.position;
            var world = Program.mainWorld;
            if (world.CheckPosition(this.position + dir))
            {
                world.Move(this.position, this.position + dir);
                this.position = this.position + dir;
                this.facing = dir;
                this.canMove = false;
                var timer = new Timer(this.symbol.ToString(), movingDelay, () => { this.canMove = true; });
            }
        }

        public virtual void TryToAttack(Vector dir)
        {
            this.facing = dir;

            if (!this.canAttack)
            {
                return;
            }

            var attack = new Attack(AttackType.Melee, this.position, this.facing, 25, this);
            this.Energy -= 30;
            this.isRestoringStamina = false;
            this.canAttack = false;
            var timer = new Timer("restStam", attack.ticks, 
                () => { this.isRestoringStamina = true; this.canAttack = true; });
        }

        public virtual void MovingLogic()
        {
            if (this.isRestoringStamina)
            {
                this.Energy += 2;
            }
        }

        public virtual void Render(RLConsole console)
        {
            console.Print(this.position.X, this.position.Y, this.symbol.ToString(), this.color);
        }

        public virtual void GetDamaged(Attack att)
        {
            this.Hp -= att.damage;
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
                    case RLKey.W: this.TryToMove(Vector.Up); break;
                    case RLKey.S: this.TryToMove(-Vector.Up); break;
                    case RLKey.A: this.TryToMove(-Vector.Right); break;
                    case RLKey.D: this.TryToMove(Vector.Right); break;

                    case RLKey.Up: this.TryToAttack(Vector.Up); break;
                    case RLKey.Down: this.TryToAttack(-Vector.Up); break;
                    case RLKey.Left: this.TryToAttack(-Vector.Right); break;
                    case RLKey.Right: this.TryToAttack(Vector.Right); break;

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


}
