using Bank.Business.Abstract;
using Bank.Business.Concrete;
using Bank.Core.Utilities.Security.Encryption;
using Bank.Core.Utilities.Security.JWT;
using Bank.DataAccess.Abstract;
using Bank.DataAccess.EntityFramework;
using Bank.DataAccess.EntityFramework.Context;
using Bank.XMLWebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddXmlSerializerFormatters();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer(options =>
    {
        var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,

            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
        };

    });

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<ISupportRequestService, SupportRequestService>();
builder.Services.AddScoped<ISupportRequestDal, EfSupportRequestDal>();


builder.Services.AddTransient<BankContext>(); 

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISupportRequestService, SupportRequestService >();
builder.Services.AddTransient<ILoginEventService, LoginEventService>();
builder.Services.AddScoped<ILoginTokenService, LoginTokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICardTransactionService, CardTransactionService>();
builder.Services.AddScoped<ILimitIncreaseService, LimitIncreaseService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IRequestLogService, RequestLogService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddHttpClient<ICurrencyExchangeService, CurrencyExchangeService>();

builder.Services.AddScoped<IUserDal, EfUserDal>();
builder.Services.AddScoped<ICardDal, EfCardDal>();
builder.Services.AddScoped<ICardTransactionDal, EfCardTransactionDal>();
builder.Services.AddScoped<IAccountDal, EfAccountDal>();
builder.Services.AddScoped<ITransactionDal, EfTransactionDal>();
builder.Services.AddScoped<IBranchDal, EfBranchDal>();
builder.Services.AddScoped<ILimitIncreaseDal, EfLimitIncreaseDal>();
builder.Services.AddScoped<IUserRoleDal, EfUserRoleDal>();
builder.Services.AddScoped<ISupportRequestDal, EfSupportRequestDal > ();
builder.Services.AddScoped<ILoginEventDal, EfLoginEventDal>();
builder.Services.AddScoped<ILoginTokenDal, EfLoginTokenDal>();


builder.Services.AddSingleton<ITokenHelper, JwtHelper>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", $"Bank API {desc.GroupName.ToUpperInvariant()}");
        }
    });
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
