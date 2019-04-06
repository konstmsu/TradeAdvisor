using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace TradeAdvisor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sgx = new SgxClient();
            var cache = new HistoricalDataCache(sgx);
            for (var i = 0; i < 100; i++)
            {
                var range = cache.GetCachedDayRange();
                var max = range.Any() ? range.Max(d => d.Index) : 1;
                await cache.EnsurePopulated(1, max + 10);
            }
        }
    }
}
