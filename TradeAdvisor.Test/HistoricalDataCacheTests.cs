using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Threading.Tasks;
using TradeAdvisor.Utils;

namespace TradeAdvisor.Test
{
    [TestClass]
    public class HistoricalDataCacheTests
    {
        [TestMethod]
        public void ShouldExtractDayIndex()
        {
            HistoricalDataCache.GetDay(new FilePath("SESprice-5416.dat")).Should().Be((Day)5416);
            HistoricalDataCache.GetDay(new FilePath("SESprice-32.dat")).Should().Be((Day)32);
        }

        [TestMethod]
        public void ShouldGetCachedDayIndexRange()
        {
            var sgx = new Mock<ISgxClient>();
            using (var directory = Temporary.GetEmptyDirectory())
                new HistoricalDataCache(sgx.Object, directory).GetCachedDayRange().Should().BeEmpty();
        }

        [TestMethod]
        public async Task ShouldCache()
        {
            var sgx = new Mock<ISgxClient>();
            sgx.Setup(c => c.GetDay(It.IsAny<Day>())).ReturnsAsync((Day day) => $"Hello day {day.Index}!");

            using (var directory = Temporary.GetEmptyDirectory())
            {
                var cache = new HistoricalDataCache(sgx.Object, directory);
                await cache.EnsurePopulated(8, 11);
                cache.GetCachedDayRange().Should().Equal(new Day[] { 8, 9, 10, 11 });
            }
        }
    }
}
