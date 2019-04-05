using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TradeAdvisor
{
    [DebuggerDisplay("{Index}")]
    public struct Day
    {
        public readonly int Index;

        public Day(int index) => Index = index;

        public static implicit operator Day(int index) => new Day(index);

        public static IReadOnlyList<Day> RangeInclusive(Day from, Day to) =>
            Enumerable.Range(from.Index, to.Index - from.Index + 1).Select(i => new Day(i)).ToList();
    }
}
