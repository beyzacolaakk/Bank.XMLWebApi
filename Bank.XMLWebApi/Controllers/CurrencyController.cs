using Bank.Business.Abstract;
using Bank.Core.Utilities.XMLSerializeToXML;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.XMLWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/xml")] // Buraya dikkat
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyExchangeService _currencyService;

        public CurrencyController(ICurrencyExchangeService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("rates")]
        public async Task<IActionResult> GetRates()
        {
            var result = await _currencyService.GetExchangeRatesAsync();

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
    }

}
