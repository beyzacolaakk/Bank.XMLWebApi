using Bank.Core.Entities.Concrete;
using Bank.Core.Utilities.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.Extensions
{
    public class RequestLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLogMiddleware> _logger;
        private readonly IRequestLogService _istekLoguServis;

        public RequestLogMiddleware(RequestDelegate next, ILogger<RequestLogMiddleware> logger, IRequestLogService istekLoguServis)  
        {
            _next = next;
            _logger = logger;
            _istekLoguServis = istekLoguServis;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var yontem = context.Request.Method;
            var yol = context.Request.Path.Value?.ToLower(); // path küçük harfe çevrildi
            var sorguParametreleri = context.Request.Query.Any()
               ? "?" + string.Join("&", context.Request.Query.Select(p => $"{p.Key}={p.Value}"))
               : "";
            var basliklar = string.Join(Environment.NewLine, context.Request.Headers.Select(h => $"{h.Key}: {h.Value}"));

            string govde = "";
            if (context.Request.ContentLength > 0 &&
                (yontem == "POST" || yontem == "PUT" || yontem == "PATCH"))
            {
                context.Request.EnableBuffering();

                using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
                {
                    govde = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
            }

            // Eğer yol giriş veya kayıt ise loglama!
            if (yol.Contains("/giris") || yol.Contains("/kayit"))
            {
                await _next(context);
                return;
            }

            int parsedId = TokendanIdAl(context);

            var log = new RequestLog
            {
                Yontem = yontem,
                Yol = yol,
                SorguParametreleri = sorguParametreleri,
                Basliklar = basliklar,
                Govde = govde,
                KullaniciId = parsedId,
                IstekZamani = DateTime.Now
            };

            try
            {
                if (parsedId != 0)
                    await _istekLoguServis.RequestLogAsync(log);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İstek logu veritabanına kaydedilirken hata oluştu.");
            }

            await _next(context);
        }

        private int TokendanIdAl(HttpContext context)
        {
            var token = context.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
                return 0;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                var userIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                    return userId;

                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }


    }
}
