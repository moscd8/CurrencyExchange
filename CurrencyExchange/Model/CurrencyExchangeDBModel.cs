using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyExchange.Model
{ 
    public class CurrencyExchangeDBModel
    {
        public Guid Id { get; set; }
        public string ExchangeName { get; set; }
        public bool Success { get; set; }

        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public float Amount { get; set; } 
        public DateTime Date { get; set; }
        public string Rate { get; set; }
         
        public float Result { get; set; } 
    }
}

