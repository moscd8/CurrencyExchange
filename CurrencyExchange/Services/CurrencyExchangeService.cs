using CurrencyExchange.Data;
using CurrencyExchange.Model;
using CurrencyExchange.Common;
using Newtonsoft.Json;  
using Microsoft.Data.SqlClient;
using System.Threading.Tasks.Dataflow;

namespace CurrencyExchange.Services
{
    public class CurrencyExchangeService  : ICurrencyExchangeService
    {
        private readonly ILogger<CurrencyExchangeService> _logger;
        private AppDBContext _context;
        private List<CurrConvert> _currConvertsList;

        public CurrencyExchangeService(AppDBContext context)
        {
            _context = context;
            InitCurrConvertsList();
        }

        private void InitCurrConvertsList()
        {
            _currConvertsList = new List<CurrConvert>(); 
            _currConvertsList.Add(new CurrConvert("USD", "ILS"));
            _currConvertsList.Add(new CurrConvert("GBP", "EUR"));
            _currConvertsList.Add(new CurrConvert("EUR", "JPY"));
            _currConvertsList.Add(new CurrConvert("EUR", "USD"));
        }

        public async Task<List<CurrencyExchangeModel>> GetAllWithAmount(float amount)
        {
            amount = amount >= 1 ? amount : 1; 
            List<CurrencyExchangeModel> listResult = new List<CurrencyExchangeModel>();
            foreach (var currConvertor in _currConvertsList)
            {
                var res = await GetRateAsync(currConvertor.Currency1, currConvertor.Currency2, amount);
                listResult.Add(res);
            }

            return listResult;
        }

        public async Task<List<CurrencyExchangeDBModel>> GetAllWithAmount2(float amount)
        {
            List<CurrencyExchangeDBModel> listResult = new List<CurrencyExchangeDBModel>();
            var fileWriterTest = new ActionBlock<CurrencyExchangeDBModel>(async (entry) =>
            {
                try
                { 
                    Console.WriteLine("Action fileWriterTest executing entry.ExchangeName={0}", entry.ExchangeName);
                    var res = await GetRateAsync(entry.FromCurrency, entry.ToCurrency, entry.Amount);                    
                    var data = CreateDBModelObj(res);
                    listResult.Add(data);
                    _context.CurrencyExchanges.Add(data);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    //TODO 
                    Console.WriteLine("Action fileWriterTest failed executing entry.ExchangeName={0} , exception={1}", entry.ExchangeName, ex);
                }
            });

            fileWriterTest.Post(GetUSDToILSObj());
            fileWriterTest.Post(GetUSDToEURObj());
            fileWriterTest.Post(GetEURToJPYObj());
            fileWriterTest.Post(GetEURToUSDObj()); 
            fileWriterTest.Complete();
            return _context.CurrencyExchanges.ToList();
        }
         
        private async Task<CurrencyExchangeModel> GetRateAsync(string currency1, string currency2, float amount)
        {
            var client = new HttpClient();

            var path = Common.Common.PATH + currency2.ToUpper() + "&from=" + currency1.ToUpper() + "&amount=" + amount;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(path),
                Headers =
                {
                    { Common.Common.APIKEY, Common.Common.KEY },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                Console.WriteLine(body);
                var newResult = JsonConvert.DeserializeObject<CurrencyExchangeModel>(body);
                Console.WriteLine("result = {0}", body);

                var data = CreateDBModelObj(newResult);

                if (data != null && data.Result >= 0)
                {
                    _context.CurrencyExchanges.Add(data);
                    _context.SaveChanges();
                }

                return newResult;
            }
        }
         
        private static CurrencyExchangeDBModel CreateDBModelObj(CurrencyExchangeModel newResult)
        {
            var currencyExchangeDBModel = new CurrencyExchangeDBModel
            {
                FromCurrency = newResult.Query.From,
                ToCurrency = newResult.Query.To,
                Amount = newResult.Query.Amount,
                Id = Guid.NewGuid(),
                ExchangeName = newResult.Query.From + "To" + newResult.Query.To,
                Rate = newResult.Info.Rate,
                Date = DateTime.Now,
                Result = newResult.Result,
                Success = newResult.Success
            };

            return currencyExchangeDBModel;
        }

        public async Task<List<string>> GetAllLatest()
        {
            var queryResult = GetLatestExchangeRateResultFromDB();
            return queryResult;
        } 

        private List<string> GetLatestExchangeRateResultFromDB()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-B2SCN7V;Initial Catalog=CurrencyExchange2-db;Integrated Security=True;Pooling=False;TrustServerCertificate=True"))
            {
                SqlCommand command = new SqlCommand($"select ExchangeName, Max(Date) as Date ,Rate FROM [CurrencyExchange2-db].[dbo].[CurrencyExchanges] group by ExchangeName, Rate", connection);
                connection.Open();
                var tempData = new List<string>();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(string.Format("{0}",
                            reader[0]));
                        var data = Enumerable.Range(0, reader.FieldCount).Select(reader.GetValue).ToArray();

                        if (data.Length == 3)
                        {
                            var output = string.Format("ExchangeName={0} , Date= {1}, Rate = {2}",
                               data[0], data[1], data[2]);
                            tempData.Add(output);
                            Console.WriteLine(output);
                        }
                    }
                }
                connection.Close();
                return tempData;
            }
        }  

        private static CurrencyExchangeDBModel GetUSDToILSObj()
        {
            var currencyExchangeDBModel = new CurrencyExchangeDBModel
            {
                FromCurrency = "USD",
                ToCurrency = "ILS",
                Amount = 1,
                Rate = "3",
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Result = 3,
                Success = true,
                ExchangeName = "USDToILS"
            };

            return currencyExchangeDBModel;
        }

        private static CurrencyExchangeDBModel GetUSDToEURObj()
        {
            var currencyExchangeDBModel = new CurrencyExchangeDBModel
            {
                FromCurrency = "USD",
                ToCurrency = "EUR",
                Amount = 1,
                Rate = "3",
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Result = 3,
                Success = true,
                ExchangeName = "USDToEUR"
            };

            return currencyExchangeDBModel;
        }

        private static CurrencyExchangeDBModel GetEURToJPYObj()
        {
            var currencyExchangeDBModel = new CurrencyExchangeDBModel
            {
                FromCurrency = "EUR",
                ToCurrency = "JPY",
                Amount = 1,
                Rate = "4",
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Result = 3,
                Success = true,
                ExchangeName = "EURToJPY"
            };

            return currencyExchangeDBModel;
        }

        private static CurrencyExchangeDBModel GetEURToUSDObj()
        {
            var currencyExchangeDBModel = new CurrencyExchangeDBModel
            {
                FromCurrency = "EUR",
                ToCurrency = "USD",
                Amount = 1,
                Rate = "2",
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Result = 3,
                Success = true,
                ExchangeName = "EURToUSD"
            };

            return currencyExchangeDBModel;
        } 
    }
}