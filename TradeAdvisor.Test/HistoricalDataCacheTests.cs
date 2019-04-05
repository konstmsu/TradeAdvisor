using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public async Task ShouldGetCachedDayIndexRange()
        {
            new HistoricalDataCache(new DirectoryInfo(".")).GetCachedDayRange().Should().BeEmpty();
        }

        [TestMethod]
        public async Task ShouldCache()
        {
            using (var directory = Temporary.GetEmptyDirectory())
            {
                var cache = new HistoricalDataCache(directory.Value);
                await cache.EnsurePopulated(8, 11);
                cache.GetCachedDayRange().Should().Equal(new[] { 8, 9, 10, 11 });
            }
        }
    }

    [TestClass]
    public class TemporaryTests
    {
        [TestMethod]
        public void ShouldDeleteDirectoryOnDispose()
        {
            var directory = Temporary.GetEmptyDirectory();
            directory.Value.Exists.Should().BeTrue();
            directory.Dispose();
            directory.Value.Exists.Should().BeFalse();
        }
    }
}
