﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace AccOperatorManager.Core
{
    public partial class AccOperator
    {
        public string Operatorid { get; set; }
        public string Name { get; set; }
        public string Line { get; set; }
        public string Op { get; set; }
        public string Status { get; set; }
        public string GroupList { get; set; }
    }
}
