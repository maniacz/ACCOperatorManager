using AccOperatorManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccOperatorManager.Data
{
    public class OracleAccOperatorData : IAccOperatorData
    {
        private readonly AccOperatorManagerDbContext db;

        public OracleAccOperatorData(AccOperatorManagerDbContext db)
        {
            this.db = db;
        }
        public IEnumerable<AccOperator> GetOperatorsByLine(Line line)
        {
            return db.Operators.Where(o => o.Line == line);
        }
    }
}
