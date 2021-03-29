using AccOperatorManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccOperatorManager.Core
{
    public class OracleAccOperatorData : IAccOperatorData
    {
        private readonly AccDbContext db;

        public OracleAccOperatorData(AccDbContext db)
        {
            this.db = db;
        }

        public string GetAllLineOps()
        {
            return db.AccOperators.OrderByDescending(o => o.Op.Length).Where(o => o.Op != null).Select(o => o.Op).FirstOrDefault();
        }

        public AccOperator GetOperatorByOperatorId(string operatorId)
        {
            return db.AccOperators.Where(o => o.Operatorid == operatorId).FirstOrDefault();
        }

        public AccOperator GetOperatorByOperatorName(string password)
        {
            return db.AccOperators.FirstOrDefault(o => o.Name == password && o.Name != null);
        }

        public IEnumerable<AccOperator> GetOperatorsByLine(LineEnum line)
        {
            return db.AccOperators.Where(o => o.Line == "Gen3_EPS2" && o.GroupList != null);
        }

        public IEnumerable<string> GetAllOperatroGroups()
        {
            return db.AccOperatorGroups.Select(g => g.GroupName);
        }

        private IEnumerable<string> GetAllNonSysGroups(IEnumerable<string> groups)
        {
            return groups.Where(g => !g.StartsWith("SYS"));
        }

        public AccOperator AddOperator(AccOperator newOperator)
        {

            List<string> allowedSysGroupsForNormalOperator = new List<string>
            {
                "SYS-ChangeStatus",
                "SYS-REPRINT",
                "SYS-TAG",
                "SYS-UNITID"
            };

            List<string> forbiddenGroupsForNormalOperator = new List<string>
            {
                "engineer",
                "inżynier",
                "inżynier/diagnosta",
                "diagnosta",
                "asystent"
            };

            var normalOperatorGroupList = GetAllOperatroGroups().Where(g => !g.StartsWith("SYS") && !forbiddenGroupsForNormalOperator.Contains(g.ToLower())).ToList();
            normalOperatorGroupList.AddRange(allowedSysGroupsForNormalOperator);

            newOperator.GroupList = string.Join(';', normalOperatorGroupList);
            newOperator.Op = GetAllLineOps();

            db.AccOperators.Add(newOperator);
            return newOperator;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }
    }
}
