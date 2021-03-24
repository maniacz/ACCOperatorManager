using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccOperatorManager.Core;
using AccOperatorManager.Core.Models;
using AccOperatorManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccOperatorManager.Pages.Operators
{
    public class ReworkOperatorsModel : PageModel
    {
        public IEnumerable<AccOperator> Operators { get; set; }


        private readonly IAccOperatorData accOperatorData;
        public ReworkOperatorsModel(IAccOperatorData accOperatorData)
        {
            this.accOperatorData = accOperatorData;
        }

        public void OnGet()
        {
            //todo: przerobiæ tak by nie by³o na sztywno B404
            Operators = accOperatorData.GetOperatorsByLine(Line.EPS_Fiat_B404);
        }
    }
}
