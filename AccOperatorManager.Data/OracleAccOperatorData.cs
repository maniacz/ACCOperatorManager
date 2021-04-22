using AccOperatorManager.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

        public OracleAccOperatorData(IConfiguration config, IDbContextFactory<AccDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
            this.config = config;
        }

        public IList<AccOperator> GetOperatorsByLine(Line line)
        {
            db = SetDbContext(line);

            //todo: przywrócić na prodzie
            return db.AccOperators.OrderBy(o => o.Operatorid).ToList();

            //return db.AccOperators.Where(o => o.Operatorid.StartsWith("test"));
        }

        private AccDbContext SetDbContext(Line line, bool isForFactoryDb = false)
        {
            string connStr = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=server_ip)(PORT=1521)))(CONNECT_DATA=(SID = server_sid)));User Id=server_user ;Password=acc";

            List<Line> lines = config.GetSection("AccLines").Get<List<Line>>();
            string accServerIp;
            string dbSid;
            string dBUser;
            //todo: wywalić poniżsżą linijkę i spr czy działa
            string schema;

            if (isForFactoryDb)
            {
                accServerIp = lines.Where(l => l.LineName == line.LineName).Select(l => l.FactoryDbIp).FirstOrDefault();
                dbSid = lines.Where(l => l.LineName == line.LineName).Select(l => l.FactoryDbSid).FirstOrDefault();
                dBUser = lines.Where(l => l.LineName == line.LineName).Select(l => l.FactoryDbUser).FirstOrDefault();
                schema = lines.Where(l => l.LineName == line.LineName).Select(l => l.FactoryDbUser).FirstOrDefault();
            }
            else
            {
                accServerIp = lines.Where(l => l.LineName == line.LineName).Select(l => l.AccServerIp).FirstOrDefault();
                dbSid = "xe";
                dBUser = "acc";
                schema = "acc";
            }

            connStr = connStr.Replace("server_ip", accServerIp);
            connStr = connStr.Replace("server_sid", dbSid);
            connStr = connStr.Replace("server_user", dBUser);

            var dbContext = dbContextFactory.CreateDbContext();

            dbContext.Database.CloseConnection();
            dbContext.Database.SetConnectionString(connStr);
            return dbContext;
        }

        public string GetAllLineOps(Line line)
        {
            //db = SetDbContext(line);
            using (var ctx = SetDbContext(line))
            {
                //ctx.Database.
                return db.AccOperators.OrderByDescending(o => o.Op.Length).Where(o => o.Op != null).Select(o => o.Op).FirstOrDefault();
            }
        }

        public AccOperator GetOperatorByOperatorId(Line line, string operatorId)
        {
            var ctx = SetDbContext(line);
            return ctx.AccOperators.Where(o => o.Operatorid == operatorId).FirstOrDefault();
        }

        public AccOperator GetOperatorByOperatorPassword(Line line, string password)
        {
            var ctx = SetDbContext(line);
            return ctx.AccOperators.FirstOrDefault(o => o.Name == password && o.Name != null);
        }

        public IEnumerable<string> GetAllOperatroGroups(Line line)
        {
            db = SetDbContext(line);
            return db.AccOperatorGroups.Select(g => g.GroupName);
        }

        private IEnumerable<string> GetAllNonSysGroups(IEnumerable<string> groups)
        {
            return groups.Where(g => !g.StartsWith("SYS"));
        }

        public AccOperator AddOperator(Line line, AccOperator newOperator)
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

            var normalOperatorGroupList = GetAllOperatroGroups(line).Where(g => !g.StartsWith("SYS") && !forbiddenGroupsForNormalOperator.Contains(g.ToLower())).ToList();
            normalOperatorGroupList.AddRange(allowedSysGroupsForNormalOperator);

            newOperator.GroupList = string.Join(';', normalOperatorGroupList);
            newOperator.Op = GetAllLineOps(line);

            //todo: może trzeba wrzucić tworzenie ctx'ów w using?
            var ctxLocalDb = SetDbContext(line);
            ctxLocalDb.AccOperators.Add(newOperator);
            ctxLocalDb.SaveChanges();

            //var ctxFactoryDb = SetDbContext(line, true);
            //ctxFactoryDb.AccOperators.Add(newOperator);
            //ctxFactoryDb.SaveChanges();

            return newOperator;
        }
        public AccOperator RemoveOperator(Line line, AccOperator operatorToRemove)
        {
            var ctxLocalDb = SetDbContext(line);
            ctxLocalDb.AccOperators.Remove(operatorToRemove);
            ctxLocalDb.SaveChanges();

            return operatorToRemove;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public IList<AccOperator> GetOperatorsWithIdStartingWith(Line line, string idStartingWith)
        {
            var ctx = SetDbContext(line);
            return ctx.AccOperators.Where(o => o.Operatorid.StartsWith(idStartingWith)).ToList();
        }
    }
}
