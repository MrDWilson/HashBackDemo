using HashBack.Models;

namespace HashBack.Services;

public interface IValidationService
{
    public ValidationResult ValidateIssuer(Request request);
    public ValidationResult ValidateCaller(Request request);
    public ValidationResult ValidateScheme(Request request);
    public ValidationResult ValidateVersion(Request request);
    public ValidationResult ValidateRandomString(Request request);
    public ValidationResult ValidateRounds(Request request);
    public ValidationResult ValidateTime(Request request);
}