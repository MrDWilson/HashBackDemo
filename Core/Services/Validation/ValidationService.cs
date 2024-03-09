using HashBack.Models;

namespace HashBack.Services;

public class ValidationService : IValidationService 
{
    private readonly IConfigurationService _configurationService;
    private readonly ICryptoService _cryptoService;

    public ValidationService(IConfigurationService configurationService,
        ICryptoService cryptoService)
    {
        _configurationService = configurationService;
        _cryptoService = cryptoService;
    }

    public ValidationResult ValidateIssuer(Request request)
    {
        var issuer = _configurationService.Get<string>("ISSUER_URL");
        if (issuer is null)
        {
            throw new ArgumentNullException("Issuer URL not found in configuration");
        }

        var uri = new Uri(request.IssuerUrl);
        var requestedIssuerUrl = $"{uri.Scheme}://{uri.Host}";
        if (requestedIssuerUrl != issuer)
        {
            return new ValidationResult
            {
                Success = false,
                Error = "Issuer URL does not match"
            };
        }

        return new ValidationResult
        {
            Success = true
        };
    }

    public ValidationResult ValidateCaller(Request request)
    {
        var callers = _configurationService.Get<List<string>>("VALID_CALLERS");
        if (callers is null)
        {
            throw new ArgumentNullException("Valid callers not found in configuration");
        }

        var uri = new Uri(request.VerifyUrl);
        var requestedCallerUrl = $"{uri.Scheme}://{uri.Host}";
        if (!callers.Contains(requestedCallerUrl))
        {
            return new ValidationResult
            {
                Success = false,
                Error = "Caller URL not known"
            };
        }

        return new ValidationResult
        {
            Success = true
        };
    }

    public ValidationResult ValidateScheme(Request request)
    {
        var issuerScheme = new Uri(request.IssuerUrl).Scheme;
        var callerScheme = new Uri(request.VerifyUrl).Scheme;

        if(issuerScheme != "https" || callerScheme != "https")
        {
            return new ValidationResult
            {
                Success = false,
                Error = "Both issuer and caller must use HTTPS"
            };
        }

        return new ValidationResult
        {
            Success = true
        };
    }

    public ValidationResult ValidateVersion(Request request)
    {
        var versions = _configurationService.Get<List<string>>("KNOWN_VERSIONS");
        if (versions is null)
        {
            throw new ArgumentNullException("HashBack version not found in configuration");
        }

        if (!versions.Contains(request.HashBack))
        {
            return new ValidationResult
            {
                Success = false,
                Error = "HashBack version not known"
            };
        }

        return new ValidationResult
        {
            Success = true
        };
    }

    public ValidationResult ValidateRandomString(Request request)
    {
        if (!_cryptoService.ValidateRandomString(request.Unus))
        {
            return new ValidationResult
            {
                Success = false,
                Error = "Unus is not valid"
            };
        }

        return new ValidationResult
        {
            Success = true
        };
    }

    public ValidationResult ValidateRounds(Request request)
    {
        var validRange = _configurationService.Get<string>("VALID_ROUND_RANGE");
        if(validRange is null)
        {
            throw new ArgumentNullException("Valid round range not found in configuration");
        }

        var parts = validRange.Split('-');
        var start = int.Parse(parts[0]);
        var end = int.Parse(parts[1]) + 1; // Range is exclusive, so add 1
        var range = new Range(start, end);

        if (request.Rounds < range.Start.Value || request.Rounds >= range.End.Value)
        {
            return new ValidationResult
            {
                Success = false,
                Error = $"Rounds must be between {range.Start.Value} and {range.End.Value}"
            };
        }

        return new ValidationResult
        {
            Success = true
        };
    }

    public ValidationResult ValidateTime(Request request)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var drift = _configurationService.Get<int?>("ALLOWED_TIME_DRIFT");
        if(drift is null)
        {
            throw new ArgumentNullException("Allowed time drift not found in configuration");
        }

        var allowedMinimum = now - drift;
        if (request.Now < allowedMinimum || request.Now > now)
        {
            return new ValidationResult
            {
                Success = false,
                Error = $"This request was not made in a valid timeframe"
            };
        }

        return new ValidationResult
        {
            Success = true
        };
    }
}