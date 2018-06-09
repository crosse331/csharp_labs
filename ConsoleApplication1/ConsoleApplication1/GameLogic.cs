using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RLNET;

namespace ConsoleApplication1
{
    class GameLogic
    {
    }

    public enum DamageType
    {
        Physical
    }

    public class Damage
    {
        private DamageType type;
        private int strenght = 0;
    }

    public enum AttackType
    {
        Melee,
    }

    public class Attack
    {
        public static readonly List<Vector> BASE_ATTACK = new List<Vector>() { Vector.Zero };
        public static readonly List<Vector> LONG_ATTACK = new List<Vector>() { Vector.Zero, Vector.Up };
        public static readonly List<Vector> WIDE_ATTACK = new List<Vector>() { Vector.Zero, Vector.Right, -Vector.Right };
        public static readonly List<Vector> LW_ATTACK = new List<Vector>() { Vector.Zero, Vector.Right, -Vector.Right, Vector.Up };

        private AttackType type;
        private Vector from;
        private Vector to;
        public int damage;

        private List<Vector> realPos = new List<Vector>();

        public int ticks = 15;

        private char symbol = (char)47;

        private Creature parent = null;
        private List<Creature> damagedCreatures = new List<Creature>();

        public Attack(AttackType tp, Vector frm, Vector t, int dmg, Creature crea, List<Vector> zone)
        {
            this.type = tp;
            this.from = frm;
            this.to = t;
            this.damage = dmg;

            this.parent = crea;

            var dir = to;

            for (int i = 0; i < zone.Count; i++)
            {
                this.realPos.Add(this.RotateVector(zone[i], dir) + from + to);
            }

            AttacksContainer.Add(this);
            var timer = new Timer("attack", ticks, () => { AttacksContainer.Remove(this); });
        }

        private Vector RotateVector(Vector def, Vector dir)
        {
            Vector tmp = def;
            if (dir == Vector.Right)
            {
                tmp.X = -def.Y;
                tmp.Y = def.X;
            }
            else if (dir == -Vector.Right)
            {
                tmp.X = def.Y;
                tmp.Y = -def.X;
            }
            else if (dir == -Vector.Up)
            {
                tmp.Y = -def.Y;
            }


            return tmp;
        }

        public void Render(RLConsole console)
        {
            for (int i = 0; i < realPos.Count; i++)
            {
                console.Print(realPos[i].X, realPos[i].Y, symbol.ToString(), RLColor.Gray);
            }
        }

        public void Logic()
        {
            switch (type)
            {
                case AttackType.Melee:
                    {
                        for (int i = 0; i < realPos.Count; i++)
                        {
                            var crea = CreaturesContainer.GetCreatureOnPosition(realPos[i]);
                            if (crea != null && crea != parent && !this.damagedCreatures.Contains(crea))
                            {
                                crea.GetDamaged(this);
                                this.damagedCreatures.Add(crea);
                            }
                        }
                        break;
                    }
            }
        }
    }

    public class CreatureAction
    {

    }
}
