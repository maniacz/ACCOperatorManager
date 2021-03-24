using AccOperatorManager.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccOperatorManager.Data
{
    public interface IAccOperatorData
    {
        IEnumerable<AccOperator> GetOperatorsByLine(Line line);
    }
}
