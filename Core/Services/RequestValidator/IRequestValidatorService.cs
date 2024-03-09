using HashBack.Models;

namespace HashBack.Services;

public interface IRequestValidatorService
{
    public ValidationResult ValidateRequest(Request request);
}