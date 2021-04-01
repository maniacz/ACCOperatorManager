using System;
using System.Collections.Generic;
using System.Text;

namespace AccOperatorManager.Core
{
    public class Line
    {
        public string LineName { get; set; }
        public string DisplayName { get; set; }
        public string AccServerIp { get; set; }
        public string FactoryDbIp { get; set; }
        public string FactoryDbSid { get; set; }
        public string FactoryDbUser { get; set; }
    }
}
