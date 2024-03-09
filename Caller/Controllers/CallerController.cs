using Microsoft.AspNetCore.Mvc;
using HashBack.Models;
using HashBack.Services;

namespace Caller.Controllers;

[ApiController]
[Route("api")]
public class CallerController : ControllerBase
{
    private readonly ILogger<CallerController> _logger;
    private readonly ICryptoService _cryptoService;
    private readonly IVerificationCacheService _verificationCacheService;
    private readonly IHttpClientFactory _clientFactory;

    private string CallerUrl => Environment.GetEnvironmentVariable("CALLER_URL")
        ?? throw new ArgumentNullException("CALLER_URL");
    private string IssuerUrl => Environment.GetEnvironmentVariable("ISSUER_URL") 
        ?? throw new ArgumentNullException("ISSUER_URL");

    public CallerController(ILogger<CallerController> logger,
        ICryptoService cryptoService,
        IVerificationCacheService verificationCacheService,
        IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _cryptoService = cryptoService;
        _verificationCacheService = verificationCacheService;
        _clientFactory = clientFactory;
    }

    [HttpGet("check/{key}")]
    public IActionResult Check(string key)
    {
        if(_verificationCacheService.Exists(key))
        {
            var value = _verificationCacheService.GetAndRemove(key);
            if(value is null)
            {
                return NotFound();
            }
            return Ok(value);
        }

        return NotFound();
    }

    [HttpGet("run")]
    public async Task<Token> Run()
    {
        var key = Guid.NewGuid().ToString();
        var request = new Request
        {
            HashBack = "HASHBACK-PUBLIC-DRAFT-3-1",
            TypeOfResponse = ResponseType.BearerToken,
            IssuerUrl = IssuerUrl + "/api/authenticate",
            Now = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Unus = _cryptoService.GenerateRandomString(),
            Rounds = 1,
            VerifyUrl = CallerUrl + "/api/check/" + key
        };

        //HASH HERE

        _verificationCacheService.Add(key, _cryptoService.GetHash(request));

        var client = _clientFactory.CreateClient();
        var response = await client.PostAsync(IssuerUrl + "/api/authenticate", JsonContent.Create(request));

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Token>()
                ?? throw new HttpRequestException("Error fetching data");
        }
        else
        {
            throw new HttpRequestException($"Error fetching data: {response.StatusCode}");
        }
    }
}
