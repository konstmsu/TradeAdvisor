using System.Linq;
using System.Threading.Tasks;

namespace TradeAdvisor
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var sgx = new SgxClient();
            var cache = new HistoricalDataCache(sgx);
            for(var i = 0; i <100; i++)
            {
                var range = cache.GetCachedDayRange();
                await cache.EnsurePopulated(0, range.Max(r => r.Index) + 10);
            }
        }
    }
}
