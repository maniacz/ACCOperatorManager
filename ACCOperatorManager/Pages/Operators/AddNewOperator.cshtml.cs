using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccOperatorManager.Core;
using AccOperatorManager.Data;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AccOperatorManager.Pages.Operators
{
    public class AddNewOperatorModel : PageModel
    {
        private readonly IAccOperatorData accOperatorData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public AccOperator NewAccOperator { get; set; }
        public IEnumerable<SelectListItem> Lines { get; set; }

        public AddNewOperatorModel(IAccOperatorData accOperatorData, IHtmlHelper htmlHelper)
        {
            this.accOperatorData = accOperatorData;
            this.htmlHelper = htmlHelper;
        }

        public IActionResult OnGet()
        {
            Lines = htmlHelper.GetEnumSelectList<Line>();

            return Page();
        }

        public IActionResult OnPost()
        {
            AccOperatorValidator validator = new AccOperatorValidator(accOperatorData);
            ValidationResult validationResult = validator.Validate(NewAccOperator);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, null);

                foreach (var failure in validationResult.Errors)
                {
                    //ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                    Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                }
            }

            //todo: Zrób przekierowanie na stronê z detalami zapisu danych
            return Page();
        }
    }
}
