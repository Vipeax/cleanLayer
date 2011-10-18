// Taken from Apoc's waypoint system
using System.Collections.Generic;

namespace cleanGatherer
{
    public class CircularQueue<T> : Queue<T>
    {
        public new T Dequeue()
        {
            T tmp = base.Dequeue();
            Enqueue(tmp);
            return tmp;
        }

        public void CycleTo(T item)
        {
            // Using a for loop here to avoid and endless cycle if no match is found!
            T tmp = Peek();
            for (int i = 0; i < Count; i++)
            {
                if (!tmp.Equals(item))
                {
                    // Toss it to the back.
                    Dequeue();
                    // Check the next one.
                    tmp = Peek();
                }
                else
                {
                    // We've hit our item. Stop here!
                    return;
                }
            }
            // No item was found, so make sure we end up where we started from.
            Dequeue();
        }
    }
}