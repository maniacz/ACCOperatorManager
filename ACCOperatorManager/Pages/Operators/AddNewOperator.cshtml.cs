using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccOperatorManager.Core;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AccOperatorManager.Pages.Operators
{
    public class AddNewOperatorModel : PageModel
    {
        private readonly IOptions<List<Line>> lines;
        private readonly IAccOperatorData accOperatorData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public AccOperator NewAccOperator { get; set; }
        [BindProperty]
        public List<string> LinesChecked { get; set; }

        //public IEnumerable<SelectListItem> Lines { get; set; }
        public IEnumerable<string> LineNames { get; set; }

        public AddNewOperatorModel(IOptions<List<Line>> lines, IAccOperatorData accOperatorData, IHtmlHelper htmlHelper)
        {
            this.lines = lines;
            this.accOperatorData = accOperatorData;
            this.htmlHelper = htmlHelper;

            PopulateLineNamesForCheckboxes();
            CreateLines();
        }

        private void PopulateLineNamesForCheckboxes()
        {
            LineNames = lines.Value.Select(l => l.DisplayName);
        }

        private void CreateLines()
        {
            lines.Value.ForEach(l => new Line());
        }

        public IActionResult OnGet()
        {
            //Lines = htmlHelper.GetEnumSelectList<LineEnum>();
            //LineNames = htmlHelper.GetEnumSelectList<LineEnum>().Select(e => e.Text);
            
            return Page();
        }

        public IActionResult OnPost()
        {
            foreach (var checkedLine in LinesChecked)
            {
                Console.WriteLine(checkedLine + " checked");
                Line line = lines.Value.FirstOrDefault(l => l.DisplayName == checkedLine);
                if (AddOperatorForLine(line))
                {
                    Console.WriteLine($"Operator added for line: {checkedLine}");
                }
                else
                {
                    Console.WriteLine($"Failed to add operator for line: {checkedLine}");
                }
            }
            return Page();
        }

        private bool AddOperatorForLine(Line line)
        {
            if (line == null)
            {
                Console.WriteLine("Line was null.");
                return false;
            }

            NewAccOperator.Line = line.LineName;

            AccOperatorValidator validator = new AccOperatorValidator(accOperatorData);
            ValidationResult validationResult = validator.Validate(NewAccOperator);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                }
                return false;
            }
            else
            {
                accOperatorData.AddOperator(NewAccOperator);
                accOperatorData.Commit();
                return true;
            }
        }
    }
}
