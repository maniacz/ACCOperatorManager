using AccOperatorManager.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccOperatorManager.Core
{
    public interface IAccOperatorData
    {
        IEnumerable<AccOperator> GetOperatorsByLine(LineEnum line);
        AccOperator GetOperatorByOperatorId(string operatorId);
        AccOperator GetOperatorByOperatorName(string password);
        string GetAllLineOps();
        IEnumerable<string> GetAllOperatroGroups();
        AccOperator AddOperator(AccOperator newOperator);
        int Commit();
    }
}
