using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccOperatorManager.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AccOperatorManager.Pages.Operators
{
    public class ReworkOperatorsModel : PageModel
    {
        public IList<AccOperator> Operators { get; set; }
        public string MaxOp { get; set; }
        public IEnumerable<string> OperatorGroups { get; set; }
        public IEnumerable<Line> Lines { get; set; }

        //[BindProperty(SupportsGet = true)] <- jak mam to g�wno tu to mi si� nie populuje dropdownlist na stronie
        public List<SelectListItem> LinesToSelect { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedLineName { get; set; }

        [BindProperty]
        public string SearchedOperator { get; set; }

        private readonly IAccOperatorData accOperatorData;
        private readonly IConfiguration config;
        private readonly ILogger logger;

        public ReworkOperatorsModel(IAccOperatorData accOperatorData, IConfiguration config, ILogger<ReworkOperatorsModel> logger)
        {
            this.accOperatorData = accOperatorData;
            this.config = config;
            this.logger = logger;
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
            return linesToSelect.OrderBy(sli => sli.Text).ToList();
        }

        private IEnumerable<Line> PopulateLinesFromAppSettings()
        {
            return config.GetSection("AccLines").Get<List<Line>>();
        }

        public IActionResult OnGet()
        {
            logger.LogInformation($"{this.GetType().Name} started method {nameof(OnGet)}");

            Line line = GetSelectedLineFromConfig(SelectedLineName);
            Operators = accOperatorData.GetOperatorsByLine(line);

            return Page();

            //MaxOp = accOperatorData.GetAllLineOps(fiatEps);
            //OperatorGroups = accOperatorData.GetAllOperatroGroups();
        }

        public void OnPost()
        {
            Line line = GetSelectedLineFromConfig(SelectedLineName);

            if (!string.IsNullOrEmpty(SearchedOperator))
            {
                Operators = accOperatorData.GetOperatorsWithIdStartingWith(line, SearchedOperator);
                return;
            }

            Operators = accOperatorData.GetOperatorsByLine(line);
        }

        private Line GetSelectedLineFromConfig(string selectedLineName)
        {
            return config.GetSection("AccLines").Get<List<Line>>().Where(l => l.LineName == selectedLineName).FirstOrDefault();
        }
    }
}
