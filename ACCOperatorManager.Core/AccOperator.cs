using System;
using System.Collections.Generic;
using System.Text;

namespace AccOperatorManager.Core
{
    public class AccOperator
    {
        //todo: tu pewnie trzeba będzie wrzucić atrybut, żeby EF sczaił, że to pole to Id
        public string OperatorId { get; set; }
        public string Name { get; set; }
        public Line Line { get; set; }
        public string Op { get; set; }
        public string Status { get; set; }
        public string GroupList { get; set; }

    }
}
