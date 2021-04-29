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
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace AccOperatorManager.Pages.Operators
{
    public class AddNewOperatorModel : PageModel
    {
        private readonly IOptions<List<Line>> lines;
        private readonly IAccOperatorData accOperatorData;
        private readonly IHtmlHelper htmlHelper;
        private readonly ILogger logger;
        private List<string> addingResult = new List<string>();

        [BindProperty]
        public AccOperator NewAccOperator { get; set; }
        [BindProperty]
        public List<string> LinesChecked { get; set; }
        [BindProperty]
        public bool NoLineSelected { get; set; }

        public IEnumerable<SelectListItem> Lines { get; set; }
        public IEnumerable<string> LineNames { get; set; }

        public AddNewOperatorModel(IOptions<List<Line>> lines, IAccOperatorData accOperatorData, IHtmlHelper htmlHelper, ILogger<AddNewOperatorModel> logger)
        {
            this.lines = lines;
            this.accOperatorData = accOperatorData;
            this.htmlHelper = htmlHelper;
            this.logger = logger;
            NoLineSelected = false;

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
            AccOperatorValidator validator = new AccOperatorValidator(accOperatorData);
            ValidationResult validationResult = validator.Validate(NewAccOperator);
            if (!validationResult.IsValid)
            {
                return Page();
            }

            if (LinesChecked.Count == 0)
            {
                NoLineSelected = true;
                return Page();
            }

            foreach (var checkedLine in LinesChecked)
            {
                Console.WriteLine(checkedLine + " checked");
                logger.LogInformation(checkedLine + " checked");
                
                Line line = lines.Value.FirstOrDefault(l => l.DisplayName == checkedLine);
                AddOperatorForLine(line);
            }

            TempData["addingResult"] = JsonSerializer.Serialize(addingResult);
            return RedirectToPage("./AddingResult");
        }

        private void AddOperatorForLine(Line line)
        {
            if (line == null)
            {
                LogAndShowMessage("Line was null.");
                return;
            }

            NewAccOperator.Line = line.LineName;

            AccOperatorValidator validator = new AccOperatorValidator(accOperatorData);
            ValidationResult validationResult = validator.Validate(NewAccOperator);
            if (!validationResult.IsValid)
            {
                LogAndShowMessage("B³¹d przy dodawaniu operatora");
                foreach (var failure in validationResult.Errors)
                {
                    LogAndShowMessage($"Niew³aœciwie wype³nione pole: {failure.PropertyName}. {failure.ErrorMessage}");
                }
            }
            else
            {
                try
                {
                    accOperatorData.AddOperator(line, NewAccOperator);
                    //accOperatorData.Commit(); - Commit przeniesiony na AccOperatorManager.Core: OracleAccOperatorData()
                    LogAndShowMessage($"Operator {NewAccOperator.Operatorid} dodany na liniê {line.DisplayName}");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.Contains("ORA-00001"))
                    {
                        accOperatorData.ChangeOperatorPassword(line, NewAccOperator);
                        LogAndShowMessage($"Ju¿ istnieje operator o takim OperatorID na linii {line.LineName}, zmienono has³o operatorowi na: {NewAccOperator.Name}");
                    }
                    else
                    {
                        LogAndShowMessage(ex.InnerException.Message ?? ex.Message);
                    }
                }
            }
        }

        private void LogAndShowMessage(string message)
        {
            Console.WriteLine(message);
            logger.LogInformation(message);
            addingResult.Add(message);
        }
    }
}
