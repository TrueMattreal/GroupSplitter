using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupSplitter.Algorithms
{
    public abstract class GroupSplitterBase<T>
    {
        public GroupSplitterBase(ChunkSizedObjects<T> objects)
        {
            Objects = objects;
        }

        public int GroupCount { get; set; }
        protected ChunkSizedObjects<T> Objects { get; }

        public abstract IEnumerable<IEnumerable<T>> SplitEven();

        protected List<T> OrderItemsDesc() => Objects.OrderByDescending(Objects.KeySelector).ToList();
        protected List<T>[] GenerateEmptyGroups() => Enumerable.Range(0, GroupCount).Select(x => new List<T>()).ToArray();
    }
}
