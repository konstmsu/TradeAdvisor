using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
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

        public async Task EnsurePopulated(Day from, Day to)
        {
            var cachedDays = GetCachedDayRange().ToHashSet();
            foreach (var day in Day.RangeInclusive(from, to))
            {
                if (cachedDays.Contains(day))
                    continue;

                await CacheDay(day);
            }
        }

        async Task CacheDay(Day day)
        {
            var cacheFile = CacheDirectory.GetFile(GetFileName(day));
            logger.Info($"Caching day {day} to file {cacheFile}...");
            CacheDirectory.Create();
            await File.WriteAllTextAsync(cacheFile.Absolute, await Sgx.GetDay(day));
        }

        string GetFileName(Day day) => $"SESprice-{day.Index}.dat";

        public HistoricalDataCache(ISgxClient sgx)
            : this(sgx, KnownPaths.SolutionRoot.GetDirectory("SGX Securities Historical"))
        {
        }

        public IReadOnlyList<Day> GetCachedDayRange() =>
            CacheDirectory.GetFiles("SESprice-*.dat").Select(GetDay).ToList().OrderBy(i => i.Index).ToList();

        public static Day GetDay(FilePath cachedDayFile) => int.Parse(Regex.Match(cachedDayFile.Name, @"\d+").Value);
    }
}
