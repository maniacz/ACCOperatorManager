using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccOperatorManager.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace AccOperatorManager.Pages.Operators
{
    public class ReworkOperatorsModel : PageModel
    {
        public IEnumerable<AccOperator> Operators { get; set; }
        public string MaxOp { get; set; }
        public IEnumerable<string> OperatorGroups { get; set; }
        public IEnumerable<Line> Lines { get; set; }

        //[BindProperty(SupportsGet = true)] <- jak mam to gówno tu to mi siê nie populuje dropdownlist na stronie
        public List<SelectListItem> LinesToSelect { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedLineName { get; set; }

        private readonly IAccOperatorData accOperatorData;
        private readonly IConfiguration config;

        public ReworkOperatorsModel(IAccOperatorData accOperatorData, IConfiguration config)
        {
            this.accOperatorData = accOperatorData;
            this.config = config;
            Lines = PopulateLinesFromAppSettings();
            LinesToSelect = PopulateLinesToSelect(Lines);
            SelectedLineName = Lines.Select(l => l.LineName).FirstOrDefault();
        }

        private List<SelectListItem> PopulateLinesToSelect(IEnumerable<Line> lines)
        {
            List<SelectListItem> linesToSelect = new List<SelectListItem>();
            foreach (var line in lines)
            {
                linesToSelect.Add(new SelectListItem { Value = line.LineName, Text = line.DisplayName });
            }
            return linesToSelect;
        }

        private IEnumerable<Line> PopulateLinesFromAppSettings()
        {
            return config.GetSection("AccLines").Get<List<Line>>();
        }

        public void OnGet()
        {
            //todo: dorobiæ jak¹œ fabrykê
            Line lineToShow = new Line();
            lineToShow.LineName = SelectedLineName;
            Operators = accOperatorData.GetOperatorsByLine(lineToShow);

            //MaxOp = accOperatorData.GetAllLineOps(fiatEps);
            //OperatorGroups = accOperatorData.GetAllOperatroGroups();
        }

        public void OnPost()
        {
            
            Line line = new Line();
            line.LineName = SelectedLineName;
            Operators = accOperatorData.GetOperatorsByLine(line);
        }
    }
}
