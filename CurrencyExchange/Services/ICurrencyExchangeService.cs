using CurrencyExchange.Model;

namespace CurrencyExchange.Services
{
    public interface ICurrencyExchangeService
    {
       Task<List<CurrencyExchangeModel>> GetAllWithAmount(float amount);
       Task<List<CurrencyExchangeDBModel>> GetAllWithAmount2(float amount);
       Task<List<string>> GetAllLatest(); 
    }
}
