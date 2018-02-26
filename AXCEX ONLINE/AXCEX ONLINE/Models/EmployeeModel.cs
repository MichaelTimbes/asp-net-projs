using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AXCEX_ONLINE.Models
{
    // Employee Model Inherits From Application User
    public class EmployeeModel : ApplicationUser
    {
        public int ID { get; set; }
        public string EMP_FNAME { get; set; }
        public string EMP_LNAME { get; set; }

    }
}
