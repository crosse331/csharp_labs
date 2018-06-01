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
        private AttackType type;
        private Vector from;
        private Vector to;
        public int damage;

        private Vector realPos;

        public int ticks = 20;

        private char symbol = (char)47;

        private Creature parent = null;
        private List<Creature> damagedCreatures = new List<Creature>();

        public Attack(AttackType tp, Vector frm, Vector t, int dmg, Creature crea)
        {
            this.type = tp;
            this.from = frm;
            this.to = t;
            this.damage = dmg;

            this.parent = crea;

            realPos = from + to;

            AttacksContainer.Add(this);
            var timer = new Timer("attack", ticks, () => { AttacksContainer.Remove(this); });
        }

        public void Render(RLConsole console)
        {
            console.Print(realPos.X, realPos.Y, symbol.ToString(), RLColor.Gray);
        }

        public void Logic()
        {
            switch (type)
            {
                case AttackType.Melee:
                    {
                        var crea = CreaturesContainer.GetCreatureOnPosition(realPos);
                        if (crea != null && crea != parent && !this.damagedCreatures.Contains(crea))
                        {
                            crea.GetDamaged(this);
                            this.damagedCreatures.Add(crea);
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
