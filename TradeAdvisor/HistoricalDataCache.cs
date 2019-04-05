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

        public HistoricalDataCache(DirectoryInfo cacheDirectory) => CacheDirectory = cacheDirectory;

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
            var client = new HttpClient();
            var response = await client.GetAsync(SgxClient.GetHistoricalUrl(day));

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);

            using (var file = new FileStream(CacheDirectory.GetFile(GetFileName(day)).FullName, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
                await response.Content.CopyToAsync(file);
        }

        string GetFileName(Day day) => $"SESprice-{day.Index}.dat";

        public HistoricalDataCache() : this(KnownPaths.SolutionRoot.GetDirectory("SGX Securities Historical")) { }

        public IReadOnlyList<Day> GetCachedDayRange() =>
            CacheDirectory.GetFiles("SESprice-*.dat").Select(GetDay).ToList().OrderBy(i => i.Index).ToList();

        public static Day GetDay(FileInfo cachedDayFile) => int.Parse(Regex.Match(cachedDayFile.Name, @"\d+").Value);
    }
}
