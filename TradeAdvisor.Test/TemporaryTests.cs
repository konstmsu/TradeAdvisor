using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TradeAdvisor.Test
{
    [TestClass]
    public class TemporaryTests
    {
        [TestMethod]
        public void ShouldDeleteDirectoryOnDispose()
        {
            var directory = Temporary.GetEmptyDirectory();
            directory.Value.Exists.Should().BeTrue();
            directory.Dispose();
            directory.Value.Refresh();
            directory.Value.Exists.Should().BeFalse();
        }
    }
}
