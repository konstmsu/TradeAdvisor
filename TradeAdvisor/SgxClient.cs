namespace TradeAdvisor
{
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
}
