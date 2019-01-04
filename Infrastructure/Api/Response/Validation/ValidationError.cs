using System;
using FluentValidation.Results;

namespace Infrastructure.Api.Response.Validation
{
    /// <summary>
    /// A serializable class to describe a validation error on request objects
    /// </summary>
    public class ValidationError
    {
        public string Message { get; set; }
        public string FieldName { get; set; }

        [Obsolete("Do not use, for serializer only")]
        public ValidationError()
        {
    
        }
        
        public ValidationError(string message)
        {
            Message = message;
        }

        public ValidationError(ValidationFailure validationFailure)
        {
            Message = validationFailure?.ErrorMessage;
            FieldName = validationFailure?.PropertyName;
        }
    }
}
