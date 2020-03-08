using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupSplitter
{
    public class ChunkSizedObjects<T> : IEnumerable<T>
    {
        public ChunkSizedObjects(IEnumerable<T> items, Func<T, int> keySelector)
        {
            Items = items;
            KeySelector = keySelector;
            Sum = Items.Sum(KeySelector);
        }

        public IEnumerable<T> Items { get; }
        public Func<T, int> KeySelector { get; }
        public int ChunkSize { get; set; } = 1;

        public int Sum { get; }

        public int GetGoal(int columns)
        {
            var goal = (int)Math.Ceiling((double)Sum / columns);
            var remainder = goal % ChunkSize;
            if (remainder > 0)
            {
                goal += ChunkSize - goal % ChunkSize;
            }
            return goal;
        }

        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();


        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
    }
}
