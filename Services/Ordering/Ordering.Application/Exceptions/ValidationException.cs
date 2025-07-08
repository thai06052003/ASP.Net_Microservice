﻿
using FluentValidation.Results;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public Dictionary<string, string[]> Errors { get; }
        public ValidationException() : base("One or more validation error(s) occurred." )
        {
           Errors = new Dictionary<string, string[]>(); 
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                     .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                     .ToDictionary(failures => failures.Key, failures => failures.ToArray());
        }
    }
}
