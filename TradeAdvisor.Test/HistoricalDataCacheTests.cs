using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TradeAdvisor.Test
{
    [TestClass]
    public class HistoricalDataCacheTests
    {
        [TestMethod]
        public void ShouldExtractDayIndex()
        {
            HistoricalDataCache.GetDay(new FileInfo("SESprice-5416.dat")).Should().Be((Day)5416);
            HistoricalDataCache.GetDay(new FileInfo("SESprice-32.dat")).Should().Be((Day)32);
        }

        [TestMethod]
        public void ShouldGetCachedDayIndexRange()
        {
            using (var directory = Temporary.GetEmptyDirectory())
                new HistoricalDataCache(directory).GetCachedDayRange().Should().BeEmpty();
        }

        [TestMethod]
        public void ShouldThrowForInvalidDays()
        {
            using (var directory = Temporary.GetEmptyDirectory())
                new HistoricalDataCache(directory).Invoking(async c => await c.EnsurePopulated(0, 1))
                    .Should().Throw<Exception>().WithMessage("No such day");

            using (var directory = Temporary.GetEmptyDirectory())
                new HistoricalDataCache(directory).Invoking(async c => await c.EnsurePopulated(9999, 9999))
                    .Should().Throw<Exception>().WithMessage("No such day");
        }

        [TestMethod]
        public async Task ShouldCache()
        {
            using (var directory = Temporary.GetEmptyDirectory())
            {
                var cache = new HistoricalDataCache(directory);
                await cache.EnsurePopulated(8, 11);
                cache.GetCachedDayRange().Should().Equal(new Day[] { 8, 9, 10, 11 });
            }
        }
    }
}
