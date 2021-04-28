using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccOperatorManager.Pages.Operators
{
    public class AddingResultModel : PageModel
    {
        public IEnumerable<string> ResultsOfAddingOperator { get; set; }
        public IActionResult OnGet()
        {
            var addingInfos = TempData["addingResult"] as string;
            if (addingInfos == null)
            {
                return RedirectToPage("./ReworkOperators");
            }
            ResultsOfAddingOperator = JsonSerializer.Deserialize<IEnumerable<string>>(addingInfos);
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("./ReworkOperators");
        }
    }
}
