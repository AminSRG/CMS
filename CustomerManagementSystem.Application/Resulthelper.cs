using FluentResults;
using FluentValidation.Results;

namespace CustomerManagementSystem.Application
{
    public static class Resulthelper
    {
        public static Result<T> AddValidationErrors<T>(this Result<T> targetResult, ValidationResult validationResult)
        {
            validationResult.Errors.ForEach(error => targetResult.WithError(error.ToString()));
            return targetResult;
        }
    }
}
