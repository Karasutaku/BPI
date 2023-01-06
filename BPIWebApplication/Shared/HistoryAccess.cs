using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared
{
    public class HistoryAccess
    {
        public string ProcedureNo { get; set; } = string.Empty;
        public string ProcedureName { get; set; } = string.Empty;
        public string UserEmail { get; set;} = string.Empty;
        public DateTime HistoryAccessDate { get; set; } = DateTime.Now;
    }
}
