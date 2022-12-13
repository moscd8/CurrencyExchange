using CurrencyExchange.Services; 
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyExchangeController : ControllerBase
    {
        private readonly ILogger<CurrencyExchangeController> _logger;
        private readonly ICurrencyExchangeService _currencyExchangeSerice; 

        public CurrencyExchangeController(ILogger<CurrencyExchangeController> logger, ICurrencyExchangeService CurrencyExchangeSerice)
        {
            _logger = logger;
            _currencyExchangeSerice = CurrencyExchangeSerice; 
        }
          
        [HttpGet("get-all")]
        public IActionResult GetAllWithAmount(float amount)
        {
            var res = _currencyExchangeSerice.GetAllWithAmount(amount);  

            return Ok(res.Result); 
        }

        [HttpGet("get-All-latest")]
        public IActionResult GetAllLatest()
        { 
            var res = _currencyExchangeSerice.GetAllLatest(); 

            return Ok(res.Result); 
        }

        [HttpGet("get-All-rates")]
        public async Task<IActionResult> GetAllData()
        { 
            var res = await _currencyExchangeSerice.GetAllWithAmount2(1);  

            return Ok(res);
        } 
    }
}