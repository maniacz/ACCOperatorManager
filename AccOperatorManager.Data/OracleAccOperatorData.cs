using AccOperatorManager.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccOperatorManager.Core
{
    public class OracleAccOperatorData : IAccOperatorData
    {
        private AccDbContext db;
        private readonly IDbContextFactory<AccDbContext> dbContextFactory;
        private readonly IConfiguration config;

        public OracleAccOperatorData(IDbContextFactory<AccDbContext> dbContextFactory, IConfiguration config)
        {
            this.dbContextFactory = dbContextFactory;
            this.config = config;
        }

        //public OracleAccOperatorData(AccDbContext db)
        //{
        //    this.db = db;
        //}

        public IEnumerable<AccOperator> GetOperatorsByLine(Line line)
        {
            db = SetDbContext(line);
            return db.AccOperators;
        }

        private AccDbContext SetDbContext(Line line)
        {
            string connStr = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=server_ip)(PORT=1521)))(CONNECT_DATA=(SID = server_sid)));User Id=server_user ;Password=acc";

            List<Line> lines = config.GetSection("AccLines").Get<List<Line>>();
            string accServerIp = lines.Where(l => l.LineName == line.LineName).Select(l => l.AccServerIp).FirstOrDefault();
            string accLocalDbSid = "xe";
            string accLocalDBUser = "acc";

            connStr = connStr.Replace("server_ip", accServerIp);
            connStr = connStr.Replace("server_sid", accLocalDbSid);
            connStr = connStr.Replace("server_user", accLocalDBUser);

            var dbContext = dbContextFactory.CreateDbContext();
            //string b415ConnStr = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.213.28.1)(PORT=1521)))(CONNECT_DATA=(SID = xe)));User Id=acc ;Password=acc";
            dbContext.Database.CloseConnection();
            dbContext.Database.SetConnectionString(connStr);
            return dbContext;
        }

        public string GetAllLineOps(Line line)
        {
            //db = SetDbContext(line);
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

        public IEnumerable<string> GetAllOperatroGroups(Line line)
        {
            //db = SetDbContext(line);
            return db.AccOperatorGroups.Select(g => g.GroupName);
        }

        private IEnumerable<string> GetAllNonSysGroups(IEnumerable<string> groups)
        {
            return groups.Where(g => !g.StartsWith("SYS"));
        }

        public AccOperator AddOperator(Line line, AccOperator newOperator)
        {
            db = SetDbContext(line);

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

            var normalOperatorGroupList = GetAllOperatroGroups(line).Where(g => !g.StartsWith("SYS") && !forbiddenGroupsForNormalOperator.Contains(g.ToLower())).ToList();
            normalOperatorGroupList.AddRange(allowedSysGroupsForNormalOperator);

            newOperator.GroupList = string.Join(';', normalOperatorGroupList);
            newOperator.Op = GetAllLineOps(line);

            db.AccOperators.Add(newOperator);
            return newOperator;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }
    }
}
