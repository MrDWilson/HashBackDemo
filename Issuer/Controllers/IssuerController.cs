using Microsoft.AspNetCore.Mvc;
using HashBack.Models;
using HashBack.Services;

namespace Issuer.Controllers;

[ApiController]
[Route("api")]
public class IssuerController : ControllerBase
{
    private readonly ILogger<IssuerController> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly ICryptoService _cryptoService;

    public IssuerController(ILogger<IssuerController> logger,
        IHttpClientFactory clientFactory,
        ICryptoService cryptoService)
    {
        _logger = logger;
        _clientFactory = clientFactory;
        _cryptoService = cryptoService;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] Request request)
    {
        var expectedHash = _cryptoService.GetHash(request);

        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync(request.VerifyUrl);

        if (response.IsSuccessStatusCode)
        {
            var actualHash = await response.Content.ReadAsStringAsync()
                ?? throw new HttpRequestException("Error fetching hash");

            if (expectedHash != actualHash)
            {
                return Unauthorized();
            }

            return Ok(new Token
            {
                BearerToken = "Success",
                IssuedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeSeconds()
            });
        }
        else
        {
            throw new HttpRequestException($"Error fetching hash: {response.StatusCode}");
        }
    }
}
