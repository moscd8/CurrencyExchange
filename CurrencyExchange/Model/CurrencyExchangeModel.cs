namespace CurrencyExchange.Model
{
    public class CurrencyExchangeModel
    { 
        public string Id { get; set; }
        public bool Success { get; set; } 
        public Query Query { get; set; } 
        public Info Info { get; set; } 
        public DateTime Date { get; set; }
        public float Result { get; set; } 
    }

    public class Query
    {
        public string From { get; set; } 
        public string To { get; set; }  
        public float Amount { get; set; }  
    }

    public class Info
    {
        public string Timestamp { get; set; }
        public string Rate { get; set; }  
    }
}

