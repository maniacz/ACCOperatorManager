using AccOperatorManager.Core;
using AccOperatorManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccOperatorManager.Data
{
    public class OracleAccOperatorData : IAccOperatorData
    {
        private readonly AccDbContext db;

        public OracleAccOperatorData(AccDbContext db)
        {
            this.db = db;
        }

        IEnumerable<AccOperator> IAccOperatorData.GetOperatorsByLine(Line line)
        {
            return db.AccOperators.Where(o => o.Line == "Gen3_EPS2" && o.Operatorid == "Diagnostyka");
        }
    }
}
