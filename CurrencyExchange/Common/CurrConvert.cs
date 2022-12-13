namespace CurrencyExchange.Common
{
    public class CurrConvert
    {
        public string Currency1 { get; set; }
        public string Currency2 { get; set; }
        public CurrConvert(string currency1, string currency2)
        {
            Currency1 = currency1;
            Currency2 = currency2;
        }
    }
}
