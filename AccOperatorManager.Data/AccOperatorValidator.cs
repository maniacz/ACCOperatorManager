using AccOperatorManager.Core;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccOperatorManager.Core
{
    public class AccOperatorValidator : AbstractValidator<AccOperator>
    {
        private readonly IAccOperatorData accOperatorData;

        public AccOperatorValidator(IAccOperatorData accOperatorData)
        {
            RuleFor(opr => opr.Operatorid).NotNull().MinimumLength(8);
            RuleFor(opr => opr.Name).NotNull().MinimumLength(6).Must(BeUniqueName).WithMessage("Takie hasło jest już zajęte.");
            this.accOperatorData = accOperatorData;
        }

        private bool BeUniqueName(string name)
        {
            return accOperatorData.GetOperatorByOperatorName(name) == null;
        }
    }
}
