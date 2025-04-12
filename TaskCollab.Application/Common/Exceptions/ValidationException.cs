using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace TaskCollab.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("Une ou plusieurs erreurs de validation se sont produites.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(
                failureGroup => failureGroup.Key, 
                failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}