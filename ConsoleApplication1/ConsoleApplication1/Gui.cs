using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLogic;
using RLNET;

namespace ConsoleApplication1
{
    class Gui
    {
    }

    public class StatsPanel
    {
        private const int PROGRESS_BAR = 220;

        public Creature target;

        public StatsPanel(Creature target)
        {
            this.target = target;
        }

        public void Render(RLConsole console)
        {
            int hpLenght = (int)target.Hp.GetMax() / 50;
            int enLenght = (int)target.Energy.GetMax() / 50;

            int hpCount = (int)Math.Round((double)target.Hp.GetCurrentCoef()) * hpLenght;
            int enCount = (int)Math.Round((double)target.Energy.GetCurrentCoef()) * enLenght;

            string hpStr = "";
            for (int i = 0; i < hpCount; i++)
            {
                hpStr += ((char)PROGRESS_BAR).ToString();
            }
            string enStr = "";
            for (int i = 0; i < enCount; i++)
            {
                enStr += ((char)PROGRESS_BAR).ToString();
            }
            console.Print(1, 1, hpStr, RLColor.Red);
            console.Print(1, 2, enStr, RLColor.Green);
        }
    }
}
