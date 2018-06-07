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
        public char symbol { get; set; }
        public RLColor color { get; set; }

        public int movingDelay = 10;
        private bool canMove = true;
        private bool canAttack = true;

        public Stat Hp = new Stat(350);
        public Stat Stamina = new Stat(300);

        private bool isRestoringStamina = true;
        private Timer staminaTimer = null;

        private List<Timer> timers = new List<Timer>();

        protected Vector prevPos;

        protected Animation currentAnimation = null;

        public Creature(char s, Vector pos)
        {
            this.symbol = s;
            this.position = pos;
            this.color = RLColor.White;

            this.Init();

            CreaturesContainer.Add(this);
        }

        public Creature(char s, Vector pos, RLColor c)
        {
            this.symbol = s;
            this.position = pos;
            this.color = c;

            this.Init();

            CreaturesContainer.Add(this);
        }

        protected virtual void Init()
        {

        }

        public virtual void TryToMove(Vector dir)
        {
            if (!this.canMove || !this.canAttack)
            {
                return;
            }
            prevPos = this.position;
            var world = Program.GetWorld();
            if (world == null)
            {
                Console.WriteLine("ALARM! World is null");
            }
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

            if (!this.canAttack || this.Stamina.GetCurrent() < 30)
            {
                return;
            }

            this.canAttack = false;
            this.isRestoringStamina = false;

            this.currentAnimation = new Animation(new List<char>() { '!' }, 10, this, () =>
             {
                 var attack = new Attack(AttackType.Melee, this.position, this.facing, 25, this);
                 this.Stamina -= 30;
                 if (this.staminaTimer != null)
                 {
                     TimersContainer.Remove(this.staminaTimer);
                     this.staminaTimer = null;
                 }
                 this.staminaTimer = new Timer("restStam", attack.ticks + 15,
                     () => { this.isRestoringStamina = true; });
                 new Timer("restCanAttack", attack.ticks - 3,
                     () => { this.canAttack = true; });
                 this.currentAnimation = null;
             });
        }

        public virtual void MovingLogic()
        {
            if (this.isRestoringStamina)
            {
                this.Stamina += 2;
            }
        }

        public virtual void Render(RLConsole console)
        {
            if (this.currentAnimation != null)
            {
                this.currentAnimation.Render();
            }
            console.Print(this.position.X, this.position.Y, this.symbol.ToString(), this.color);
        }

        public virtual void GetDamaged(Attack att)
        {
            this.Hp -= att.damage;
            if (this.Hp.GetCurrent() <= 0)
            {
                CreaturesContainer.Remove(this);
            }
        }
    }

    public class Player : Creature
    {
        public Player(char s, Vector pos) : base(s, pos) { }
        public Player(char s, Vector pos, RLColor c) : base(s, pos, c) { }

        private RLKey lastPressedKey = RLKey.Unknown;

        protected override void Init()
        {
            base.Init();

            this.movingDelay = 2;
        }

        public override void MovingLogic()
        {
            base.MovingLogic();

            var keyPress = Program.MainConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                if (keyPress.Key == lastPressedKey || keyPress.Repeating)
                {
                    return;
                }
                lastPressedKey = keyPress.Key;
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
            else
            {
                lastPressedKey = RLKey.Unknown;
            }
        }
    }

    public class TestEnemy : Creature
    {
        public TestEnemy(char s, Vector pos) : base(s, pos) { }
        public TestEnemy(char s, Vector pos, RLColor c) : base(s, pos, c) { }

        private Random randomizer = new Random();

        protected override void Init()
        {
            base.Init();

            this.movingDelay = 10;
        }

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
