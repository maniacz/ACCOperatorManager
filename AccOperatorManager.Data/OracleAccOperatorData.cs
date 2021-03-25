using AccOperatorManager.Core;
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

        public AccOperator GetOperatorByOperatorId(string operatorId)
        {
            return db.AccOperators.Where(o => o.Operatorid == operatorId).FirstOrDefault();
        }

        public AccOperator GetOperatorByOperatorName(string password)
        {
            return db.AccOperators.FirstOrDefault(o => o.Name == password && o.Name != null);
        }

        public IEnumerable<AccOperator> GetOperatorsByLine(Line line)
        {
            return db.AccOperators.Where(o => o.Line == "Gen3_EPS2" && o.GroupList != null);
        }
    }
}
