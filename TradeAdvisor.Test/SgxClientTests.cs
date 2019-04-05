using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TradeAdvisor.Test
{
    [TestClass]
    public class SgxClientTests
    {
        [TestMethod]
        public void ShouldDownload()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ShouldGetUrl()
        {
            //SgxClient.GetHistoricalUrl(new DateTime(2019, 04, 04)).Should().Be("https://links.sgx.com/1.0.0/securities-historical/5416/SESprice.dat");
            //SgxClient.GetHistoricalUrl(new DateTime(2019, 04, 01)).Should().Be("https://links.sgx.com/1.0.0/securities-historical/5413/SESprice.dat");
            SgxClient.GetHistoricalUrl(5416).Should().Be("https://links.sgx.com/1.0.0/securities-historical/5416/SESprice.dat");
            SgxClient.GetHistoricalUrl(5413).Should().Be("https://links.sgx.com/1.0.0/securities-historical/5413/SESprice.dat");
        }
    }
}
