using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccOperatorManager.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccOperatorManager.Pages.Operators
{
    public class ReworkOperatorsModel : PageModel
    {
        public IEnumerable<AccOperator> Operators { get; set; }
        public string MaxOp { get; set; }
        public IEnumerable<string> OperatorGroups { get; set; }


        private readonly IAccOperatorData accOperatorData;
        public ReworkOperatorsModel(IAccOperatorData accOperatorData)
        {
            this.accOperatorData = accOperatorData;
        }

        public void OnGet()
        {
            Line b403 = new Line()
            {
                LineName = "TSSP_VW_EPS4",
                DisplayName = "B403"
            };
            Line fiatEps = new Line()
            {
                LineName = "Gen3_EPS",
                DisplayName = "6103"
            };

            Operators = accOperatorData.GetOperatorsByLine(fiatEps);

            MaxOp = accOperatorData.GetAllLineOps(fiatEps);
            //OperatorGroups = accOperatorData.GetAllOperatroGroups();
        }
    }
}
