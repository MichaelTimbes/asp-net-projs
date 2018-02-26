using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AXCEX_ONLINE.Models
{
    public class AdminModel : ApplicationUser
    {
        public int ID { get; set; }
        public string ADMIN_NAME { get; set; }

    }
}
