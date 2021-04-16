using AccOperatorManager.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccOperatorManager.Core
{
    public interface IAccOperatorData
    {
        IEnumerable<AccOperator> GetOperatorsByLine(Line line);
        AccOperator GetOperatorByOperatorId(Line line, string operatorId);
        AccOperator GetOperatorByOperatorName(string password);
        string GetAllLineOps(Line line);
        IEnumerable<string> GetAllOperatroGroups(Line line);
        AccOperator AddOperator(Line line, AccOperator newOperator);
        AccOperator RemoveOperator(Line line, AccOperator operatorToRemove);
        int Commit();
    }
}
