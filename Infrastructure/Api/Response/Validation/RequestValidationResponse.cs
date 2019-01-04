using System.Collections.Generic;
using FluentValidation.Results;

namespace Infrastructure.Api.Response.Validation
{
    /// <summary>
    /// Encapsulates a valid response using some sort of validation extensions
    /// We want this so we can swap out validation tools and still keep a standard response
    /// </summary>
    public class RequestValidationResponse
    {
        public bool IsValid { get; set; }
        public IList<ValidationError> ValidationErrors { get; set; }

        /// <summary>
        /// False - sets default validation error message
        /// True - ValidationErrors array is null
        /// </summary>
        /// <param name="isValid"></param>
        public RequestValidationResponse(bool isValid)
        {
            IsValid = isValid;
            if (!isValid)
            {
                ValidationErrors = new List<ValidationError>
                {
                    new ValidationError("Request is null or required sub objects are null")
                };
            }
        }

        /// <summary>
        /// Sets to false, if you need to pass in a message validation has failed
        /// </summary>
        /// <param name="message"></param>
        public RequestValidationResponse(string message)
        {
            IsValid = false;
            ValidationErrors = new List<ValidationError>
            {
                new ValidationError(message)
            };
        }

        /// <summary>
        /// Translate a 3rd party FluentValidation Result to our own Validation Response
        /// </summary>
        /// <param name="validationResult"></param>
        public RequestValidationResponse(ValidationResult validationResult)
        {
            IsValid = validationResult.IsValid;
            ValidationErrors = new List<ValidationError>();
            foreach (var validationResultError in validationResult.Errors)
            {
                var apiError = new ValidationError(validationResultError);
                ValidationErrors.Add(apiError);
            }
        }
    }
}
