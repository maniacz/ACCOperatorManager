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
        public void OnGet()
        {
            var addingInfos = TempData["addingResult"] as string;
            ResultsOfAddingOperator = JsonSerializer.Deserialize<IEnumerable<string>>(addingInfos);
        }
    }
}
