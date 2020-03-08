using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupSplitter.Algorithms
{
    public class GreedyAlgorithm<T> : GroupSplitterBase<T>
    {
        public GreedyAlgorithm(ChunkSizedObjects<T> objects) : base(objects)
        {
        }

        /// <summary>
        /// Performance is bad, but it's worse than the other algorithm(s) so i will not optimize it.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<IEnumerable<T>> SplitEven()
        {
            var orderedItems = OrderItemsDesc();
            var groups = GenerateEmptyGroups();
            foreach (var item in orderedItems)
            {
                var smallestGroup = groups.OrderBy(g => g.Sum(Objects.KeySelector)).First();
                smallestGroup.Add(item);
            }
            return groups;
        }
    }
}
