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
        public async Task ShouldCache()
        {
            var sgx = new Mock<ISgxClient>(MockBehavior.Strict);
            sgx.SetupSequence(c => c.GetDay(It.IsAny<Day>())).ReturnsAsync("Hello!");

            using (var directory = Temporary.GetEmptyDirectory())
            {
                var cache = new HistoricalDataCache(sgx.Object, directory);
                Day day = 1;
                var data = await cache.GetDay(day);
                File.ReadAllText(cache.GetCacheFile(day).Absolute).Should().Be(data);
            }
        }
    }
}
