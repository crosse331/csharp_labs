using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RLNET;

namespace ConsoleApplication1
{
    public class Container<T>
    {
        protected static List<T> container = new List<T>();

        public static void Add(T item)
        {
            container.Add(item);
        }

        public static void Remove(T item)
        {
            if (container.Contains(item))
            {
                container.Remove(item);
            }
        }
    }

    public class CreaturesContainer : Container<Creature>
    {
        public static void MovingLogic()
        {
            for (int i = 0; i < container.Count; i++)
            {
                container[i].MovingLogic();
            }
        }

        public static void RenderLogic(RLConsole console)
        {
            for (int i = 0; i < container.Count; i++)
            {
                container[i].Render(console);
            }
        }

        public static Creature GetCreatureOnPosition(Vector pos)
        {
            Creature result = null;
            for (int i = 0; i < container.Count; i++)
            {
                if (container[i].position == pos)
                {
                    result = container[i];
                }
            }

            return result;
        }

        public static Player GetPlayer()
        {
            for (int i=0;i< container.Count;i++)
            {
                if (container[i] is Player)
                {
                    return container[i] as Player;
                }
            }

            return null;
        }
    }

    public  class TimersContainer : Container<Timer>
    {
        public static void Logic()
        {
            for (int i = 0; i < container.Count; i++)
            {
                container[i].Logic();
            }
        }
    }

    public class AttacksContainer : Container<Attack>
    {
        public static void Logic()
        {
            for (int i=0;i<container.Count;i++)
            {
                container[i].Logic();
            }
        }

        public static void Render(RLConsole console)
        {
            for (int i = 0; i < container.Count; i++)
            {
                container[i].Render(console);
            }
        }
    }
}
