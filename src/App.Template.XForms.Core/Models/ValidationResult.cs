using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace App.Template.XForms.Core.Models
{
    public class ValidateResult
    {
        public bool HasErrors => Failures != null && Failures.Any();
        public IList<ValidationFailure> Failures { get; set; }
    }
}