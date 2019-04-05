using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TradeAdvisor
{
    public static class KnownPaths
    {
        public static DirectoryInfo SolutionRoot => solutionRoot.Value;
        static readonly Lazy<DirectoryInfo> solutionRoot = new Lazy<DirectoryInfo>(FindSolutionRoot);

        static DirectoryInfo FindSolutionRoot()
        {
            var current = new DirectoryInfo(Directory.GetCurrentDirectory());

            for (; ; )
            {
                if (current == null)
                    throw new Exception();

                if (current.GetFile("TradeAdvisor.sln").Exists)
                    return current;

                current = current.Parent;
            }
        }
    }

    public class SgxClient
    {
        // 2019-04-01 == 5413
        // 2019-04-04 == 5416
        public static string GetHistoricalUrl(Day day)
        {
            var dayIndex = day.Index;
            return $"https://links.sgx.com/1.0.0/securities-historical/{dayIndex}/SESprice.dat";
        }
    }

    [DebuggerDisplay("{Index}")]
    public struct Day
    {
        public readonly int Index;

        public Day(int index) => Index = index;

        public static implicit operator Day(int index) => new Day(index);

        public static IReadOnlyList<Day> RangeInclusive(Day from, Day to) =>
            Enumerable.Range(from.Index, to.Index - from.Index + 1).Select(i => new Day(i)).ToList();
    }

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

    class Program
    {
        static async Task Main(string[] args)
        {
            var cache = new HistoricalDataCache();
            for(var i = 0; i <100; i++)
            {
                var range = cache.GetCachedDayRange();
                await cache.EnsurePopulated(0, range.Max(r => r.Index) + 10);
            }
        }
    }

    public static class ExtensionMethods
    {
        public static FileInfo GetFile(this DirectoryInfo directory, string fileName) =>
            new FileInfo(Path.Combine(directory.FullName, fileName));
        public static DirectoryInfo GetDirectory(this DirectoryInfo directory, string childName) =>
            new DirectoryInfo(Path.Combine(directory.FullName, childName));
    }
}
