using System;
using System.IO;
using System.Threading.Tasks;
using NLog;
using TradeAdvisor.Utils;

namespace TradeAdvisor
{
    public class HistoricalDataCache
    {
        static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        readonly DirectoryPath CacheDirectory;
        readonly ISgxClient Sgx;

        public HistoricalDataCache(ISgxClient sgx, DirectoryPath cacheDirectory)
        {
            CacheDirectory = cacheDirectory;
            Sgx = sgx;
        }

        public async Task<string> GetDay(Day day)
        {
            var (cacheFile, content) = await EsureDayCached(day);
            return content();
        }

        public async Task<(FilePath cacheFile, Func<string> content)> EsureDayCached(Day day)
        {
            var cacheFile = GetCacheFile(day);

            if (cacheFile.Exists)
            {
                logger.Debug($"Found {day} cached as {cacheFile}");
                return (cacheFile, () => File.ReadAllText(cacheFile.Absolute));
            }

            CacheDirectory.Create();
            var data = await Sgx.GetDay(day);

            logger.Info($"Caching {day} to file {cacheFile}...");
            await File.WriteAllTextAsync(cacheFile.Absolute, data);
            return (cacheFile, () => data);
        }

        public FilePath GetCacheFile(Day day) => CacheDirectory.GetFile($"SESprice-{day.Index}.dat");

        public HistoricalDataCache(ISgxClient sgx)
            : this(sgx, KnownPaths.SolutionRoot.GetDirectory("data/sgx"))
        {
        }
    }
}
