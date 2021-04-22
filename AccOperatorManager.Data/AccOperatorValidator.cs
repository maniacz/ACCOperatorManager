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
            this.accOperatorData = accOperatorData;
            RuleFor(opr => opr.Operatorid).NotNull().MinimumLength(8);
            RuleFor(opr => opr.Name).NotNull().MinimumLength(6);

            //todo: Dorobić walidację hasła pod różne dbContexty
            //RuleFor(opr => opr.Name).NotNull().MinimumLength(6).Must(BeUniqueName).WithMessage("Takie hasło jest już zajęte.");
        }

        //private bool BeUniqueName(string name)
        //{
        //    return accOperatorData.GetOperatorByOperatorName(name) == null;
        //    //return true;
        //}
    }
}
