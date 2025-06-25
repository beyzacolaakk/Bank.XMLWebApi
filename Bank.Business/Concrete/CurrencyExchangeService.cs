using Bank.Business.Abstract;
using Bank.Core.Utilities.Results;
using Bank.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Business.Concrete
{
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        private readonly HttpClient _httpClient;

        public CurrencyExchangeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IDataResult<ExchangeRatesResponse>> GetExchangeRatesAsync()
        {
            try
            {
                string url = "https://api.frankfurter.app/latest?from=TRY";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return new ErrorDataResult<ExchangeRatesResponse>($"HTTP error: {response.StatusCode}");

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (!root.TryGetProperty("rates", out var ratesElement))
                    return new ErrorDataResult<ExchangeRatesResponse>("Response JSON does not contain 'rates' property.");

                var rates = new Dictionary<string, decimal>();

                foreach (var property in ratesElement.EnumerateObject())
                {
                    if (property.Value.ValueKind == JsonValueKind.Number && property.Value.TryGetDecimal(out var rateValue))
                    {
                        // Tersini alıyoruz: 1 Döviz birimi kaç TRY eder
                        if (rateValue != 0)
                            rates[property.Name] = Math.Round(1 / rateValue, 4);
                        else
                            rates[property.Name] = 0;
                    }
                    else
                    {
                        return new ErrorDataResult<ExchangeRatesResponse>($"Invalid rate value for currency {property.Name}");
                    }
                }

                DateTime date = root.TryGetProperty("date", out var dateProp) ? dateProp.GetDateTime() : DateTime.Now;

                // Dictionary'den XML uyumlu listeye çeviriyoruz
                var ratesList = rates.Select(kv => new RateItem
                {
                    Currency = kv.Key,
                    Value = kv.Value
                }).ToList();

                var result = new ExchangeRatesResponse
                {
                    BaseCurrency = "TRY",
                    Date = date,
                    Rates = ratesList
                };

                return new SuccessDataResult<ExchangeRatesResponse>(result);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<ExchangeRatesResponse>($"Exception: {ex.Message}");
            }
        }




    }
}
