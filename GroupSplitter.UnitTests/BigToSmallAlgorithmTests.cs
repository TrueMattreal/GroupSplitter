using GroupSplitter.Algorithms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace GroupSplitter.UnitTests
{
    public class BigToSmallAlgorithmTests
    {
        private readonly ITestOutputHelper output;

        public BigToSmallAlgorithmTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData( 1,  10, 1,    10, 3, 1.00)] // ca. 97%
        [InlineData( 1, 100, 1,    10, 3, 1.00)] // ok
        [InlineData(10, 100, 1,    10, 3, 0.95)] // ca. 16% :(
        [InlineData(1,   10, 1, 1_000, 3, 1.00)] // ok
        [InlineData(1,  100, 1, 1_000, 3, 0.95)] // ca. 80%
        [InlineData(10, 100, 1, 1_000, 3, 0.95)] // ca. 81%
        // chunks 
        public void SplitEvenTest(int min, int max, int chunkSize, int itemsCount, int groupCount, double targetedPrecision)
        {
            //File.Delete(@".\notSplittedEvent.csv");
            var totalFails = 0;
            var total = 100_000;
            for (var i = 0; i < total; i++)
            {
                var random = new Random();
                var items = Enumerable
                    .Repeat(0, itemsCount)
                    .Select(i => random.Next(min, max) * chunkSize)
                    .ToArray();
                var originalSum = items.Sum();
                static int KeySelector(int x) => x;
                IEnumerable<int>[] evenlySplittedGroups;
                var objects = new ChunkSizedObjects<int>(items, KeySelector)
                {
                    ChunkSize = chunkSize
                };
                evenlySplittedGroups = new BigToSmallAlgorithm<int>(objects)
                {
                    GroupCount = groupCount
                }.SplitEven().ToArray();
                
                Assert.Equal(originalSum, evenlySplittedGroups.SelectMany(x => x).Sum());

                var goal = objects.GetGoal(groupCount);
                try
                {
                    foreach (var group in evenlySplittedGroups)
                    {
                        Assert.InRange(group.Sum(), goal - chunkSize * 2, goal + chunkSize);
                    }
                }
                catch
                {
                    totalFails++;
                    var lines = new List<string>();
                    lines.Add(string.Join(';', items));
                    for (var j = 0; j < groupCount; j++)
                    {
                        lines.Add($"[{j}, {evenlySplittedGroups[j].Sum()}]:{string.Join(';', evenlySplittedGroups[j])}");
                    }
                    lines.Add($"Sum: {originalSum}; Goal: {goal}\n");
                    //File.AppendAllLines(@".\notSplittedEvent.csv", lines);
                }
            }
            var precision = 1 - (totalFails / (double)total);
            output.WriteLine(precision.ToString());
            Assert.InRange(precision, targetedPrecision, 1);
            //File.AppendAllText(@".\notSplittedEvent.csv", $"TotalFails: {totalFails}");
        }
    }
}
