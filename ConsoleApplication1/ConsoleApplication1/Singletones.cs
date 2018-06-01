using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RLNET;

namespace ConsoleApplication1
{
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

    public static class TimersContainer
    {
        public static List<Timer> allTimers = new List<Timer>();

        public static void AddTimer(Timer t)
        {
            allTimers.Add(t);
        }

        public static void Logic()
        {
            for (int i = 0; i < allTimers.Count; i++)
            {
                allTimers[i].Logic();
            }
        }

        public static void Remove(Timer t)
        {
            if (allTimers.Contains(t))
            {
                allTimers.Remove(t);
            }
        }
    }
}
