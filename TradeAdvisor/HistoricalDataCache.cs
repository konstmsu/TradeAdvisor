using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TradeAdvisor
{
    public class HistoricalDataCache
    {
        readonly DirectoryInfo CacheDirectory;
        readonly ISgxClient Sgx;

        public HistoricalDataCache(ISgxClient sgx, DirectoryInfo cacheDirectory)
        {
            CacheDirectory = cacheDirectory;
            Sgx = sgx;
        }

        public async Task EnsurePopulated(Day from, Day to)
        {
            CacheDirectory.Create();
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
            var response = await Sgx.GetDay(day);
            await File.WriteAllTextAsync(CacheDirectory.GetFile(GetFileName(day)).FullName, response);
        }

        string GetFileName(Day day) => $"SESprice-{day.Index}.dat";

        public HistoricalDataCache(ISgxClient sgx)
            : this(sgx, KnownPaths.SolutionRoot.GetDirectory("SGX Securities Historical"))
        {
        }

        public IReadOnlyList<Day> GetCachedDayRange() =>
            CacheDirectory.GetFiles("SESprice-*.dat").Select(GetDay).ToList().OrderBy(i => i.Index).ToList();

        public static Day GetDay(FileInfo cachedDayFile) => int.Parse(Regex.Match(cachedDayFile.Name, @"\d+").Value);
    }
}
