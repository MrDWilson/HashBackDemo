using HashBack.Models;

namespace HashBack.Services;

public class RequestValidatorService : IRequestValidatorService
{
    private readonly IValidationService _validationService;

    public RequestValidatorService(IValidationService validationService)
    {
        _validationService = validationService;
    }

    public ValidationResult ValidateRequest(Request request)
    {
        var issuerValidation = _validationService.ValidateIssuer(request);
        if(!issuerValidation.Success)
        {
            return issuerValidation;
        }

        var callerValidation = _validationService.ValidateCaller(request);
        if(!callerValidation.Success)
        {
            return callerValidation;
        }

        // Docker HTTPS is a bit of a pain to set up, so we'll skip this for now
        //var schemeValidation = _validationService.ValidateScheme(request);
        //if(!schemeValidation.Success)
        //{
        //    return schemeValidation;
        //}

        var versionValidation = _validationService.ValidateVersion(request);
        if(!versionValidation.Success)
        {
            return versionValidation;
        }

        var randomStringValidation = _validationService.ValidateRandomString(request);
        if(!randomStringValidation.Success)
        {
            return randomStringValidation;
        }

        var roundsValidation = _validationService.ValidateRounds(request);
        if(!roundsValidation.Success)
        {
            return roundsValidation;
        }

        var timeValidation = _validationService.ValidateTime(request);
        if(!timeValidation.Success)
        {
            return timeValidation;
        }

        return new ValidationResult
        {
            Success = true
        };
    }
}