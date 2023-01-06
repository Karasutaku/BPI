using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.ReportModel
{
    public class AccessHistoryReport
    {
        public DateTime startDate { get; set; } = DateTime.Now;
        public DateTime endDate { get; set; } = DateTime.Now;
        public string procedureNo { get; set; } = string.Empty;
    }
}
