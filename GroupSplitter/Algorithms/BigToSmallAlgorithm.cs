using GroupSplitter.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GroupSplitter
{
    public class BigToSmallAlgorithm<T> : GroupSplitterBase<T>
    {
        public BigToSmallAlgorithm(ChunkSizedObjects<T> objects) : base(objects)
        {
        }

        public override IEnumerable<IEnumerable<T>> SplitEven()
        {
            var orderedItems = OrderItemsDesc();
            var groups = GenerateEmptyGroups();
            foreach (var group in groups)
            {
                FillGroup(Objects, orderedItems, group);
                RemoveRange(orderedItems, group);
            }
            // If not every item in a group, sort the remaing items manually
            AddRemaingItems(orderedItems, groups);
            return groups;
        }

        private List<T> FillGroup(ChunkSizedObjects<T> items, List<T> orderedItems, List<T> group)
        {
            var curSum = 0;
            foreach (var item in orderedItems)
            {
                if (curSum + items.KeySelector(item) > items.GetGoal(GroupCount))
                    continue;
                group.Add(item);
                curSum += items.KeySelector(item);
            }
            return group;
        }
        private static void RemoveRange(List<T> items, List<T> remove)
        {
            foreach (var item in remove)
            {
                items.Remove(item);
            }
        }
        private void AddRemaingItems(List<T> source, List<T>[] destination)
        {
            if (source.Count <= 0)
            {
                return;
            }
            foreach (var item in source)
            {
                var smallestGroup = destination.OrderBy(g => g.Sum(Objects.KeySelector)).First();
                smallestGroup.Add(item);
            }
        }
    }
}
