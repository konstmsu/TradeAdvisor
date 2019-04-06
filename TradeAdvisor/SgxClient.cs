using System;
using System.Net.Http;
using System.Threading.Tasks;
using NLog;

namespace TradeAdvisor
{
    public interface ISgxClient
    {
        Task<string> GetDay(Day day);
    }

    public class SgxClient : ISgxClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static string GetHistoricalUrl(Day day)
        {
            // 2019-04-01 == 5413
            // 2019-04-04 == 5416
            var dayIndex = day.Index;
            return $"https://links.sgx.com/1.0.0/securities-historical/{dayIndex}/SESprice.dat";
        }

        public async Task<string> GetDay(Day day)
        {
            var url = GetHistoricalUrl(day);
            logger.Info($"Loading day {day} from {url}...");

            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (content.Contains(@"<span id=""lblErrorMsg"">No Record Found.</span>"))
                throw new HttpRequestException("No Record Found");

            return content;
        }
    }
}
