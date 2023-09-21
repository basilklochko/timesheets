using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet.Library.Model
{
    public class Token
    {
        public int id { get; set; }
        public Guid Guid { get; set; }
        public int UserId { get; set; }
    }
}
