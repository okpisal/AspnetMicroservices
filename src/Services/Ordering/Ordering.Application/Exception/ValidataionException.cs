using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exception
{
    public class ValidataionException :ApplicationException
    {
        public ValidataionException() 
        :base("One or More validataion failures have occurred.")
        { }

        public ValidataionException(IEnumerable<ValidationFailure> failures)
            :this()
        { 
            Errors=failures
                    .GroupBy(e=>e.PropertyName, e=>e.ErrorMessage)
                    .ToDictionary(failureGroup=>failureGroup.Key, failureGroup =>failureGroup.ToArray());
        }
        public IDictionary<string, string[]> Errors { get;}
    }
}
