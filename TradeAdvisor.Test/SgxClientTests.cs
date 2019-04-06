using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TradeAdvisor.Test
{
    [TestClass]
    public class SgxClientTests
    {
        [TestMethod]
        public async Task ShouldGetDay()
        {
            var sgx = new SgxClient();
            var response = await sgx.GetDay(1);
            response.Should().Contain("2013-03-22;800 SUPER HOLDINGS LIMITED");
        }

        [TestMethod]
        public void ShouldGetUrl()
        {
            SgxClient.GetHistoricalUrl(Day._2019_04_04).Should().Be("https://links.sgx.com/1.0.0/securities-historical/5416/SESprice.dat");
            SgxClient.GetHistoricalUrl(Day._2019_04_01).Should().Be("https://links.sgx.com/1.0.0/securities-historical/5413/SESprice.dat");
        }

        [TestMethod]
        public void ShouldThrowForInvalidDays()
        {
            var sgx = new SgxClient();
            void TestNoRecord(Func<Task> test) => test.Should().Throw<HttpRequestException>().WithMessage("No Record Found");
            TestNoRecord(() => sgx.GetDay(0));
            TestNoRecord(() => sgx.GetDay(9999));
        }
    }
}
