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
    public class QueueList
    {
        private Queue<Action<Dictionary<string, string>, object>> queue = new Queue<Action<Dictionary<string, string>, object>>();
        public void Add(Action<Dictionary<string, string>, object> p)
        {
            queue.Enqueue(p);
        }

        public Action<Dictionary<string, string>, object> Get()
        {
            return queue.Dequeue();
        }

        public bool IsGet(Action<Dictionary<string, string>, object> p)
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
