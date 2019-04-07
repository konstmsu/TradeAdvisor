namespace TradeAdvisor
{
    public struct Day
    {
        public readonly int Index;
        public static readonly Day _2019_04_04 = 5416;
        public static readonly Day _2019_04_01 = 5413;

        public Day(int index) => Index = index;

        public override string ToString() => $"day {Index}";

        public static implicit operator Day(int index) => new Day(index);
    }
}
