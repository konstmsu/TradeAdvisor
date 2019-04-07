using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NLog;
using TradeAdvisor.Utils;

namespace TradeAdvisor
{
    class Program
    {
        static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        static async Task Main(string[] args)
        {
            var sgx = new SgxClient();
            var cache = new HistoricalDataCache(sgx);
            var knownMissingDays = new Day[]
            {
                3801, 3869, 3888,
                3910, 3911, 3912, 3913, 3914, 3915, 3916, 3917, 3918,
                4382, 4564, 4730, 5156 
            };

            for (var i = 1; i < 5420; i++)
            {
                Day day = i;

                if (knownMissingDays.Contains(day))
                    continue;

                try
                {
                    await cache.GetDay(day);
                }
                catch (HttpRequestException ex)
                {
                    logger.Warn($"Failed to load {day}", ex);
                    var failedDaysFile = new FilePath("FailedDays");
                    var failedDays = failedDaysFile.ReadLines().Select(int.Parse).ToHashSet();
                    failedDays.Add(day.Index);
                    await failedDaysFile.WriteLines(failedDays.ToList().OrderBy(d => d).Select(d => $"{d}"));
                }
            }
        }
    }
}
