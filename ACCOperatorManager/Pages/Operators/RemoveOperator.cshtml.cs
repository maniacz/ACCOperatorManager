using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccOperatorManager.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace AccOperatorManager.Pages.Operators
{
    public class RemoveOperatorModel : PageModel
    {
        private readonly IAccOperatorData accOperatorData;
        private readonly IOptions<List<Line>> lines;

        public AccOperator accOperator { get; set; }
        public Line line { get; set; }

        public RemoveOperatorModel(IAccOperatorData accOperatorData, IOptions<List<Line>> lines)
        {
            this.accOperatorData = accOperatorData;
            this.lines = lines;
        }

        public IActionResult OnGet(string lineName, string operatorId)
        {
            line = lines.Value.Where(l => l.LineName == lineName).FirstOrDefault();
            if (line == null)
            {
                //todo: przerobiæ tak by strona not found wyœwietla³a info, ¿e nie zanleziono takiej linii 4 overload RedirectToPage
                return RedirectToPage("./NotFound");
            }

            accOperator = accOperatorData.GetOperatorByOperatorId(line, operatorId);
            if (accOperator == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost(string lineName, string operatorId)
        {
            line = lines.Value.Where(l => l.LineName == lineName).FirstOrDefault();
            if (line == null)
            {
                //todo: przerobiæ tak by strona not found wyœwietla³a info, ¿e nie zanleziono takiej linii 4 overload RedirectToPage
                return RedirectToPage("./NotFound");
            }

            accOperator = accOperatorData.GetOperatorByOperatorId(line, operatorId);
            if (accOperator == null)
            {
                return RedirectToPage("./NotFound");
            }

            accOperatorData.RemoveOperator(line, accOperator);
            return RedirectToPage("./ReworkOperators");
        }
    }
}
