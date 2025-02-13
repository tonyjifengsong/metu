using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Created by tony
    /// </summary>
    public class QueueReturnList
    {
        private Queue<Func<Dictionary<string, string>, object>> queue = new Queue<Func<Dictionary<string, string>, object>>();
        public void Add(Func<Dictionary<string, string>, object> p)
        {
            queue.Enqueue(p);
        }

        public Func<Dictionary<string, string>, object> Get()
        {
            return queue.Dequeue();
        }

        public bool IsGet(Func<Dictionary<string, string>, object> p)
        {
            bool resule = false;
            resule = queue.Contains(p);
            return resule;
        }

        public bool IsHaveElement()
        {
            if (queue.Count <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int GetQueueCount()
        {
            return queue.Count;
        }
    }
}
