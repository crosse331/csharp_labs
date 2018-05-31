using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class LogicObject
    {
        public LogicObject()
        {
            //LogicobjectContainer
        }

        public virtual void Update()
        {

        }
    }

    public class LogicobjectContainer
    {
        private List<LogicObject> objects = new List<LogicObject>();

        public void AddObject(LogicObject obj)
        {
            this.objects.Add(obj);
        }
    }
}
